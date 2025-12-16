import pygame
import random
from save import load, save
from game_state import game_state


class Ground(pygame.sprite.Sprite):
    """A sprite class for the ground tilemap."""
    
    # Soil tile mask dictionary mapping neighbor configurations to tile indices
    SOIL_TILE_MASKS = {
        (False, False, False, False): 9,      # none
        (True, True, True, True): 10,         # up_down_left_right
        (True, False, False, False): 11,      # up
        (False, True, False, False): 12,      # down
        (False, False, True, False): 13,      # left
        (False, False, False, True): 14,      # right
        (True, False, True, False): 15,       # up_left
        (False, True, True, False): 16,       # down_left
        (False, True, False, True): 17,       # down_right
        (True, False, False, True): 18,       # up_right
        (True, False, True, True): 19,        # up_left_right
        (True, True, False, True): 20,        # up_down_right
        (False, True, True, True): 21,        # down_right_left
        (True, True, True, False): 22,        # up_down_left
        (True, True, False, False): 23,       # up_down
        (False, False, True, True): 24,       # left_right
    }
    
    def __init__(self):
        """Initialize the ground sprite.
        
        Args:
            world_width: Width of the world in pixels
            world_height: Height of the world in pixels
            tile_size: Size of each tile in pixels (default 16x16)
        """
        super().__init__()
        
        # Load the tilemap
        self.tilemap = pygame.image.load(r"..\Assets\Tilemaps\GroundTilemap.png").convert_alpha()
        self.tile_frames = self._extract_tile_frames()
        
        # Load or generate tiles
        player_data = load()
        if player_data.get("grass_tiles") and len(player_data["grass_tiles"]) == game_state.total_tiles:
            self.grass_tiles = player_data["grass_tiles"]
        else:
            self.grass_tiles = self._generate_grass_tiles()
            # Save the generated tiles
            player_data["grass_tiles"] = self.grass_tiles
            save(player_data)
        
        # Load or generate soil tiles
        if player_data.get("soil_tiles"):
            self.soil_tiles = set(player_data["soil_tiles"])
        else:
            self.soil_tiles = set()
            player_data["soil_tiles"] = []
            save(player_data)
        
        # Create the background surface
        self.image = pygame.Surface((game_state.world_width, game_state.world_height))
        self.rect = self.image.get_rect()
        self._render_tiles()
    
    def _extract_tile_frames(self):
        """Extract all 25 tiles from the tilemap (5x5 grid of 16x16 tiles)."""
        frames = []
        tilemap_width = self.tilemap.get_width()
        tilemap_height = self.tilemap.get_height()
        tile_width = tilemap_width // 5  # 5x5 grid of tiles
        tile_height = tilemap_height // 5
        
        # Extract all 25 tiles (0-24)
        for row in range(5):
            for col in range(5):
                x = col * tile_width
                y = row * tile_height
                frame = self.tilemap.subsurface((x, y, tile_width, tile_height))
                frames.append(frame)
        
        return frames
    
    def _random_grass_tile(self):
        """Return a random tile index (0-8) with 70% chance of 0, 30% chance of others."""
        if random.random() < 0.7:
            return 0
        else:
            return random.randint(1, 8)
    
    def _generate_grass_tiles(self):
        """Generate random grass tiles with 70% chance of tile 0, 30% chance of others."""
        grass_tiles = []
        for _ in range(self.total_tiles):
            grass_tiles.append(self._random_grass_tile())
        return grass_tiles
    
    def _get_mask(self, tile_idx):
        """Calculate the correct soil tile mask based on neighboring grass tiles.
        
        Args:
            tile_idx: The tile index to calculate the mask for
            
        Returns:
            A tuple of (up, down, left, right) booleans indicating which neighbors are grass
        """
        col = tile_idx % game_state.num_tiles_x
        row = tile_idx // game_state.num_tiles_x
        
        # Check neighbors - True if neighbor is GRASS (not soil)
        has_up = row > 0 and (tile_idx - game_state.num_tiles_x) not in self.soil_tiles
        has_down = row < game_state.num_tiles_y - 1 and (tile_idx + game_state.num_tiles_x) not in self.soil_tiles
        has_left = col > 0 and (tile_idx - 1) not in self.soil_tiles
        has_right = col < game_state.num_tiles_x - 1 and (tile_idx + 1) not in self.soil_tiles
        
        return (has_up, has_down, has_left, has_right)
    
    def _get_soil_tile_num(self, tile_idx):
        """Get the correct soil tile number based on neighboring tiles.
        
        Args:
            tile_idx: The tile index to get the soil tile for
            
        Returns:
            The soil tile index (9-24)
        """
        mask = self._get_mask(tile_idx)
        return self.SOIL_TILE_MASKS.get(mask, 9)
    
    def _render_tiles(self):
        """Render all tiles onto the background surface."""
        for idx, tile_num in enumerate(self.grass_tiles):
            # Calculate position in grid
            col = idx % game_state.num_tiles_x
            row = idx // game_state.num_tiles_x
            x = col * game_state.tile_size
            y = row * game_state.tile_size
            
            # Determine if the tile is soil or grass
            if idx in self.soil_tiles:
                tile_num = self._get_soil_tile_num(idx)
            
            # Scale tile frame to fit tile size
            tile_frame = self.tile_frames[tile_num]
            scaled_tile = pygame.transform.scale(tile_frame, (game_state.tile_size, game_state.tile_size))
            
            # Blit onto background
            self.image.blit(scaled_tile, (x, y))
    
    def _render_tile(self, tile_idx):
        """Render a single tile at the given index."""
        col = tile_idx % game_state.num_tiles_x
        row = tile_idx // game_state.num_tiles_x
        x = col * game_state.tile_size
        y = row * game_state.tile_size
        
        tile_num = self.grass_tiles[tile_idx]
        if tile_idx in self.soil_tiles:
            tile_num = self._get_soil_tile_num(tile_idx)
        
        tile_frame = self.tile_frames[tile_num]
        scaled_tile = pygame.transform.scale(tile_frame, (game_state.tile_size, game_state.tile_size))
        self.image.blit(scaled_tile, (x, y))
    
    def _update_affected_tiles(self, tile_idx):
        """Update the tile and all its neighbors to reflect smart tile changes.
        
        Args:
            tile_idx: The tile index that changed
        """
        col = tile_idx % game_state.num_tiles_x
        row = tile_idx // game_state.num_tiles_x
        
        # Re-render the changed tile
        self._render_tile(tile_idx)
        
        # Re-render all neighbors if they are soil tiles
        neighbors = []
        if row > 0:
            neighbors.append(tile_idx - game_state.num_tiles_x)  # up
        if row < game_state.num_tiles_y - 1:
            neighbors.append(tile_idx + game_state.num_tiles_x)  # down
        if col > 0:
            neighbors.append(tile_idx - 1)  # left
        if col < game_state.num_tiles_x - 1:
            neighbors.append(tile_idx + 1)  # right
        
        for neighbor_idx in neighbors:
            if neighbor_idx in self.soil_tiles:
                self._render_tile(neighbor_idx)
    
    def on_ground_click(self, world_pos):
        """Handle a click on the ground when the hoe is active.
        
        Args:
            world_pos: Tuple of (x, y) world coordinates where the click occurred
        """
        if not game_state.is_hoe_active():
            return
        
        self._apply_hoe_to_tile(world_pos)
    
    def update_drag(self, world_pos):
        """Update dragging by applying hoe to tile at current position.
        
        Args:
            world_pos: Tuple of (x, y) world coordinates where the mouse currently is
        """
        if not game_state.is_dragging_hoe():
            return
        
        action = game_state.get_drag_action()
        self._apply_hoe_to_tile(world_pos, action)
    
    def _apply_hoe_to_tile(self, world_pos, action=None):
        """Apply the hoe to a single tile at the given world position.
        
        Args:
            world_pos: Tuple of (x, y) world coordinates
            action: "to_soil" to convert to soil, "to_grass" to convert to grass, 
                   or None to toggle based on current tile
        """
        # Convert world position to tile index
        tile_x = int(world_pos[0] // game_state.tile_size)
        tile_y = int(world_pos[1] // game_state.tile_size)
        
        # Check bounds
        if tile_x < 0 or tile_x >= game_state.num_tiles_x or tile_y < 0 or tile_y >= game_state.num_tiles_y:
            return
        
        # Calculate tile index
        tile_idx = tile_y * game_state.num_tiles_x + tile_x
        
        # Determine what to do based on action or current tile
        if action == "to_soil":
            # Convert to soil if not already soil
            if tile_idx not in self.soil_tiles:
                self.soil_tiles.add(tile_idx)
                self._update_affected_tiles(tile_idx)
        elif action == "to_grass":
            # Convert to grass if not already grass
            if tile_idx in self.soil_tiles:
                self.soil_tiles.remove(tile_idx)
                self._update_affected_tiles(tile_idx)
        else:
            # Toggle behavior (for single clicks without initial action)
            if tile_idx in self.soil_tiles:
                self.soil_tiles.remove(tile_idx)
            else:
                self.soil_tiles.add(tile_idx)
            self._update_affected_tiles(tile_idx)
    
    def _save_tiles(self):
        """Save the current tiles to the save file."""
        player_data = load()
        player_data["grass_tiles"] = self.grass_tiles
        player_data["soil_tiles"] = list(self.soil_tiles)
        save(player_data)
    
    def update(self, *args, **kwargs):
        """Update method (required by Sprite class)."""
        pass
