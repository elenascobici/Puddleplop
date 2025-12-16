"""
Main game loop for the Frog game with camera functionality.
"""
import pygame
from frog import Frog, extract_frames
from ground import Ground
from hoe import Hoe
from seed_satchel import SeedSatchel
from stats import CoinBar, XpBar
from save import load
from game_state import game_state

# Pygame setup
pygame.init()

# Initialize game state with screen dimensions
screen = pygame.display.set_mode((game_state.base_width, game_state.base_height), pygame.RESIZABLE)
pygame.display.set_caption("Puddleplop")

clock = pygame.time.Clock()
running = True
dt = 0

# Load player data from save file
player_data = load()

# Create sprite group
all_sprites = pygame.sprite.Group()

# Create the ground
ground = Ground()
all_sprites.add(ground)

# Create the frog player
player = Frog()
all_sprites.add(player)

# Create the hoe icon
hoe_icon = Hoe(icon_size=32)

# Create the seed satchel icon
satchel_icon = SeedSatchel(icon_size=32)

# Create the stat bars
coin_bar = CoinBar(screen.get_width(), coins=player_data.get("coins", 0))
xp_bar = XpBar(screen.get_width(), level=player_data.get("level", 1))

# Camera position
camera_x, camera_y = 0, 0

# Track fullscreen state
is_fullscreen = False


def calculate_scale_and_offset(screen_w, screen_h, game_w, game_h):
    """Calculate scale and offset for letterboxing/pillarboxing.
    
    Returns:
        scale: The uniform scale factor
        offset_x: X offset to center the game
        offset_y: Y offset to center the game
    """
    scale_x = screen_w / game_w
    scale_y = screen_h / game_h
    scale = min(scale_x, scale_y)  # Use smaller scale to fit within screen
    
    scaled_w = game_w * scale
    scaled_h = game_h * scale
    
    offset_x = (screen_w - scaled_w) / 2
    offset_y = (screen_h - scaled_h) / 2
    
    return scale, offset_x, offset_y


# Initial scale and offset calculation
current_scale, screen_offset_x, screen_offset_y = calculate_scale_and_offset(
    screen.get_width(), screen.get_height(), game_state.base_width, game_state.base_height
)

# Update the game loop to handle camera movement
while running:
    # Poll for events
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            running = False
        elif event.type == pygame.KEYDOWN:
            if event.key == pygame.K_F11:
                # Toggle fullscreen
                is_fullscreen = not is_fullscreen
                if is_fullscreen:
                    screen = pygame.display.set_mode((0, 0), pygame.FULLSCREEN)
                else:
                    screen = pygame.display.set_mode((game_state.base_width, game_state.base_height), pygame.RESIZABLE)
                # Recalculate scale and offset
                current_scale, screen_offset_x, screen_offset_y = calculate_scale_and_offset(
                    screen.get_width(), screen.get_height(), game_state.base_width, game_state.base_height
                )
        elif event.type == pygame.VIDEORESIZE:
            # Handle window resize - recalculate scale and offset
            current_scale, screen_offset_x, screen_offset_y = calculate_scale_and_offset(
                screen.get_width(), screen.get_height(), game_state.base_width, game_state.base_height
            )
        elif event.type == pygame.MOUSEBUTTONDOWN:
            # Convert mouse position from screen space to game space
            mouse_x = (event.pos[0] - screen_offset_x) / current_scale
            mouse_y = (event.pos[1] - screen_offset_y) / current_scale
            adjusted_pos = (mouse_x, mouse_y)
            
            # Check if hoe icon was clicked
            if hoe_icon.rect.collidepoint(adjusted_pos):
                hoe_icon.on_click()
                satchel_icon.update_image()
                hoe_icon.update_position()
                satchel_icon.update_position()
            # Check if satchel icon was clicked
            elif satchel_icon.rect.collidepoint(adjusted_pos):
                satchel_icon.on_click()
                hoe_icon.update_image()
                hoe_icon.update_position()
                satchel_icon.update_position()
            # Check if ground was clicked (and hoe is active)
            elif game_state.is_hoe_active():
                # Convert to world coordinates for ground
                world_x = adjusted_pos[0] / (game_state.base_width / game_state.camera_width)
                world_y = adjusted_pos[1] / (game_state.base_height / game_state.camera_height)
                world_pos = (camera_x + world_x, camera_y + world_y)
                
                # Determine what tile was clicked
                tile_x = int(world_pos[0] // ground.tile_size)
                tile_y = int(world_pos[1] // ground.tile_size)
                
                # Check bounds
                if 0 <= tile_x < ground.tiles_wide and 0 <= tile_y < ground.tiles_high:
                    tile_idx = tile_y * ground.tiles_wide + tile_x
                    
                    # Determine action based on whether tile is soil or grass
                    if tile_idx in ground.soil_tiles:
                        action = "to_grass"
                    else:
                        action = "to_soil"
                    
                    # Start drag with the determined action
                    game_state.start_drag(action)
                    # Apply hoe to initial click
                    ground.on_ground_click(world_pos)
        elif event.type == pygame.MOUSEBUTTONUP:
            # End drag and save tiles
            if game_state.is_dragging_hoe():
                ground._save_tiles()
            game_state.end_drag()

    # Fill the screen with black for fullscreen borders
    screen.fill("black")

    # Create a game surface at base resolution
    game_surface = pygame.Surface((game_state.base_width, game_state.base_height))
    game_surface.fill("purple")

    # Handle input and update player
    keys = pygame.key.get_pressed()
    player.update(keys, game_state.world_width, game_state.world_height, dt)

    # Update ground dragging if hoe is being dragged
    if game_state.is_dragging_hoe():
        # Get current mouse position and convert to world coordinates
        mouse_screen_x, mouse_screen_y = pygame.mouse.get_pos()
        mouse_x = (mouse_screen_x - screen_offset_x) / current_scale
        mouse_y = (mouse_screen_y - screen_offset_y) / current_scale
        world_x = mouse_x / (game_state.base_width / game_state.camera_width)
        world_y = mouse_y / (game_state.base_height / game_state.camera_height)
        world_pos = (camera_x + world_x, camera_y + world_y)
        ground.update_drag(world_pos)

    # Update the camera position to center on the player, clamped to world bounds
    camera_x = max(0, min(game_state.world_width - game_state.camera_width, player.pos.x + game_state.frog_width / 2 - game_state.camera_width / 2))
    camera_y = max(0, min(game_state.world_height - game_state.camera_height, player.pos.y + game_state.frog_height / 2 - game_state.camera_height / 2))

    # Scale factor for world rendering (not affected by screen scaling)
    scale_x = game_state.base_width / game_state.camera_width
    scale_y = game_state.base_height / game_state.camera_height

    # Draw the ground tiles relative to the camera
    ground_screen_x = (0 - camera_x) * scale_x
    ground_screen_y = (0 - camera_y) * scale_y
    scaled_ground = pygame.transform.scale(ground.image, (int(game_state.world_width * scale_x), int(game_state.world_height * scale_y)))
    game_surface.blit(scaled_ground, (ground_screen_x, ground_screen_y))

    # Draw the frog sprite relative to the camera
    frog_screen_x = (player.pos.x - camera_x) * scale_x
    frog_screen_y = (player.pos.y - camera_y) * scale_y
    scaled_sprite = pygame.transform.scale(player.image, (int(game_state.frog_width * scale_x), int(game_state.frog_height * scale_y)))
    game_surface.blit(scaled_sprite, (frog_screen_x, frog_screen_y))

    # Draw the hoe icon (fixed to screen, not affected by camera)
    game_surface.blit(hoe_icon.image, hoe_icon.rect)

    # Draw the seed satchel icon (fixed to screen, not affected by camera)
    game_surface.blit(satchel_icon.image, satchel_icon.rect)

    # Draw the stat bars (fixed to screen, not affected by camera)
    game_surface.blit(coin_bar.image, coin_bar.rect)
    game_surface.blit(xp_bar.image, xp_bar.rect)

    # Scale the game surface to fit the screen with letterboxing/pillarboxing
    scaled_game_surface = pygame.transform.scale(
        game_surface,
        (int(game_state.base_width * current_scale), int(game_state.base_height * current_scale))
    )
    screen.blit(scaled_game_surface, (screen_offset_x, screen_offset_y))

    # Flip the display to put your work on screen
    pygame.display.flip()

    # Limit FPS to 60
    dt = clock.tick(60) / 1000

pygame.quit()