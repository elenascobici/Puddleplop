"""
Main game loop for the Frog game with camera functionality.
"""
import pygame
from frog import Frog, extract_frames
from ground import Ground
from shop_outside import ShopOutside
from shop_door import ShopDoor
from hoe import Hoe
from seed_satchel import SeedSatchel
from employee_menu_icon import EmployeeMenuIcon
from employee_menu_book import EmployeeMenuBook
from stats import CoinBar, XpBar
from save import load
from game_state import game_state
from shared import DragAction, Scenes

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

# Create sprite groups
all_sprites = pygame.sprite.Group()
switch_scene_sprites = pygame.sprite.Group() # Sprites that trigger scene switches
active_sprites = pygame.sprite.Group() # Sprites that are actively being updated in the game loop

# Create the ground
ground = Ground()
all_sprites.add(ground)

# Create the frog player
player = Frog()
all_sprites.add(player)

# Create the shop outside
shop_outside = ShopOutside()
all_sprites.add(shop_outside)
switch_scene_sprites.add(shop_outside)

# Creat the shop door leading outside
shop_door = ShopDoor()
# all_sprites.add(shop_door)
# switch_scene_sprites.add(shop_door)

# Create the hoe icon
hoe_icon = Hoe(icon_size=32)

# Create the seed satchel icon
satchel_icon = SeedSatchel(icon_size=32)

# Create the employee menu icon
employee_menu_icon = EmployeeMenuIcon(icon_size=48)

# Create the employee menu book
employee_menu_book = EmployeeMenuBook()

outside_sprites = [ground, shop_outside, hoe_icon, satchel_icon]
shop_sprites = [shop_door, employee_menu_icon]

# Create the stat bars
coin_bar = CoinBar(screen.get_width(), coins=player_data.get("coins", 0))
xp_bar = XpBar(screen.get_width(), level=player_data.get("level", 1), xp=player_data.get("xp", 0))

# Camera position
camera_x, camera_y = 0, 0

# Track fullscreen state
is_fullscreen = False

# Assets
hardwood_floor = pygame.image.load(r"..\Assets\Tiles\ShopInterior\HardwoodFloor.png").convert_alpha()

# HELPERS
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

def render_to_scale(object):
    """Helper to render an object to the game surface with scaling."""
    screen_x = (object.rect.x - camera_x) * game_state.scale_x
    screen_y = (object.rect.y - camera_y) * game_state.scale_y
    scaled_image = pygame.transform.scale(object.image, (int(object.rect.width * game_state.scale_x), int(object.rect.height * game_state.scale_y)))
    game_surface.blit(scaled_image, (screen_x, screen_y))

# Initial scale and offset calculation
current_scale, screen_offset_x, screen_offset_y = calculate_scale_and_offset(
    screen.get_width(), screen.get_height(), game_state.base_width, game_state.base_height
)

# GAME LOOP
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
            
            if game_state.current_scene == Scenes.OUTSIDE:
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
                    tile_x = int(world_pos[0] // game_state.tile_size)
                    tile_y = int(world_pos[1] // game_state.tile_size)
                    
                    # Check bounds
                    if 0 <= tile_x < game_state.num_tiles_x and 0 <= tile_y < game_state.num_tiles_y:
                        tile_idx = tile_y * game_state.num_tiles_x + tile_x
                        
                        # Determine action based on whether tile is soil or grass
                        if tile_idx in ground.soil_tiles:
                            action = DragAction.TO_GRASS
                        else:
                            action = DragAction.TO_SOIL
                        
                        # Start drag with the determined action
                        game_state.start_drag(action)
                        # Apply hoe to initial click
                        ground.on_ground_click(world_pos)
            elif game_state.current_scene == Scenes.SHOP:
                # Check if employee menu icon was clicked
                if employee_menu_icon.rect.collidepoint(adjusted_pos):
                    employee_menu_icon.on_click()
                    if game_state.employee_menu_open:
                        employee_menu_book.reset()
                        active_sprites.add(employee_menu_book)
        elif event.type == pygame.MOUSEBUTTONUP and game_state.current_scene == Scenes.OUTSIDE:
            # End drag and save tiles
            if game_state.is_dragging_hoe():
                ground._save_tiles()
            game_state.end_drag()

    # Fill the screen with black for fullscreen borders
    screen.fill("black")

    # Create a game surface at base resolution
    game_surface = pygame.Surface((game_state.base_width, game_state.base_height))

    # Handle input and update player
    keys = pygame.key.get_pressed()
    player.update(keys, dt)

    # Check if player is entering a new scene
    if pygame.sprite.spritecollideany(player, switch_scene_sprites):
        if player.rect.colliderect(shop_outside.rect): # Entering the shop from outside
            for item in outside_sprites: item.kill()
            for item in shop_sprites: item.add(all_sprites)
            shop_door.add(switch_scene_sprites)

            game_state.set_scene(Scenes.SHOP)
            player.set_position(game_state.camera_width - 50, game_state.camera_height)
            player.velocity = pygame.math.Vector2(0, 0)
            player.current_frame = 0
            player.last_direction = "front" 
        elif player.rect.colliderect(shop_door.rect): # Exiting the shop to outside
            for item in outside_sprites: item.add(all_sprites)
            for item in shop_sprites: item.kill()
            shop_outside.add(switch_scene_sprites)

            game_state.set_scene(Scenes.OUTSIDE)
            player.set_position(game_state.world_width / 2, game_state.world_height / 2)
            player.velocity = pygame.math.Vector2(0, 0)
            player.current_frame = 0
            player.last_direction = "front" 
            

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
    game_state.scale_x = game_state.base_width / game_state.camera_width
    game_state.scale_y = game_state.base_height / game_state.camera_height

    # Render background based on scene
    if game_state.current_scene == Scenes.OUTSIDE:
        render_to_scale(ground)
        render_to_scale(shop_outside)
    elif game_state.current_scene == Scenes.SHOP:
        # Fill the shop interior with hardwood floor tiles
        scaled_hardwood_floor = pygame.transform.scale(
            hardwood_floor,(int(hardwood_floor.get_width() * game_state.scale_x), int(hardwood_floor.get_height() * game_state.scale_y)))
        tile_w, tile_h = scaled_hardwood_floor.get_size()

        for x in range(0, game_state.base_width, tile_w):
            for y in range(0, game_state.base_height, tile_h):
                game_surface.blit(scaled_hardwood_floor, (x, y))

        # Draw the door leading back outside
        render_to_scale(shop_door)

    # Render common elements (shown for all scenes)
    # Draw the frog sprite relative to the camera
    frog_screen_x = (player.pos.x - camera_x) * game_state.scale_x
    frog_screen_y = (player.pos.y - camera_y) * game_state.scale_y
    scaled_sprite = pygame.transform.scale(player.image, (int(game_state.frog_width * game_state.scale_x), int(game_state.frog_height * game_state.scale_y)))
    game_surface.blit(scaled_sprite, (frog_screen_x, frog_screen_y))

    # Draw the stat bars (fixed to screen, not affected by camera)
    game_surface.blit(coin_bar.image, coin_bar.rect)
    game_surface.blit(xp_bar.image, xp_bar.rect)

    # Render foreground based on scene
    if game_state.current_scene == Scenes.OUTSIDE:
        # Icons that are fixed on the screen
        game_surface.blit(hoe_icon.image, hoe_icon.rect)
        game_surface.blit(satchel_icon.image, satchel_icon.rect)
    elif game_state.current_scene == Scenes.SHOP:
        # Draw the employee menu icon fixed on the screen
        game_surface.blit(employee_menu_icon.image, employee_menu_icon.rect)

    active_sprites.update(dt)
    active_sprites.draw(game_surface)

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