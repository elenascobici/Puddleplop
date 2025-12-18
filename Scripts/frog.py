"""
A module defining the Frog (player) sprite for the game.
"""
import pygame
from game_state import game_state
from shared import extract_frames

class Frog(pygame.sprite.Sprite):
    """A sprite class for the frog player character."""
    
    def __init__(self):
        """Initialize the frog sprite.
        
        Args:
            x: Starting x position in world coordinates
            y: Starting y position in world coordinates
            all_frames: Dictionary of all frames for each direction
            moving_frames: Dictionary of moving animation frames for each direction
            idle_frames: Dictionary of idle frames for each direction
            sprite_width: Width of the sprite in pixels
            sprite_height: Height of the sprite in pixels
            speed: Movement speed in pixels per second
        """
        super().__init__()

        # Load the frog sprite sheets
        front_sheet = pygame.image.load(r"..\Assets\Characters\Frog\FrogSpritesheetFront.png").convert_alpha()
        back_sheet = pygame.image.load(r"..\Assets\Characters\Frog\FrogSpritesheetBack.png").convert_alpha()
        side_sheet = pygame.image.load(r"..\Assets\Characters\Frog\FrogSpritesheetSide.png").convert_alpha()

        self.all_frames = {
            "front": extract_frames(front_sheet, 5, 5, 22, game_state.frog_width, game_state.frog_height),
            "back": extract_frames(back_sheet, 4, 4, 15, game_state.frog_width, game_state.frog_height),
            "side": extract_frames(side_sheet, 4, 4, 16, game_state.frog_width, game_state.frog_height),
        }

        # Define animation frames for each direction
        # Front: images 5-9, Back: images 4-9, Side: images 6-11
        self.moving_frames = {
            "front": self.all_frames["front"][5:10],  # Frames 5-9 (5 frames)
            "back": self.all_frames["back"][4:10],    # Frames 4-9 (6 frames)
            "side": self.all_frames["side"][6:12],    # Frames 6-11 (6 frames)
        }

        # Idle frames (index 0 for each direction when stationary)
        self.idle_frames = {
            "front": self.all_frames["front"][0],
            "back": self.all_frames["back"][0],
            "side": self.all_frames["side"][0],
        }
        
        self.pos = pygame.Vector2(game_state.world_width / 2, game_state.world_height / 2)
        self.speed = 200
        
        # Animation state
        self.direction = "front"
        self.last_direction = "front"
        self.flip_sprite = False
        self.animation_timer = 0
        self.animation_speed = 0.05  # Time per frame in seconds
        self.current_frame = 0
        
        # Set initial image and rect
        self.image = pygame.transform.scale(
            self.idle_frames["front"],
            (game_state.frog_width, game_state.frog_height)
        )
        self.rect = self.image.get_rect(topleft=(game_state.world_width / 2, game_state.world_height / 2))

    def set_position(self, x, y):
        """Set the frog's position in world coordinates."""
        self.pos.x = x
        self.pos.y = y
        
        # Reset to avoid wonky behaviour when changing scenes
        self.velocity = pygame.math.Vector2(0, 0)
        self.current_frame = 0
        self.last_direction = "front" 
    
    def update(self, keys, dt):
        """Update the frog's position and animation.
        
        Args:
            keys: pygame.key.get_pressed() result
            dt: Delta time in seconds
        """
        if not game_state.frog_moving_enabled:
            return
        # Create velocity vector for movement
        velocity = pygame.Vector2(0, 0)
        moving = False
        
        # Accumulate movement input
        if keys[pygame.K_w]:
            velocity.y -= 1
            self.direction = "back"
            moving = True
        if keys[pygame.K_s]:
            velocity.y += 1
            self.direction = "front"
            moving = True
        if keys[pygame.K_a]:
            velocity.x -= 1
            self.direction = "side"
            self.flip_sprite = True
            moving = True
        if keys[pygame.K_d]:
            velocity.x += 1
            self.direction = "side"
            self.flip_sprite = False
            moving = True
        
        # Normalize velocity to prevent faster diagonal movement
        if velocity.length() > 0:
            velocity = velocity.normalize()
        
        # Apply normalized velocity to position
        self.pos += velocity * self.speed * dt
        
        # Keep the frog within world bounds
        self.pos.x = max(0, min(game_state.world_width - game_state.frog_width, self.pos.x))
        self.pos.y = max(0, min(game_state.world_height - game_state.frog_height, self.pos.y))
        
        # Update animation
        if self.direction != self.last_direction:
            self.last_direction = self.direction
            self.current_frame = 0
            self.animation_timer = 0
            sprite_to_draw = self.moving_frames[self.direction][self.current_frame]
        elif moving: 
            self.animation_timer += dt
            if self.animation_timer >= self.animation_speed:
                self.animation_timer = 0
                self.current_frame = (self.current_frame + 1) % len(self.moving_frames[self.direction])
            sprite_to_draw = self.moving_frames[self.direction][self.current_frame]
        else:
            self.current_frame = 0
            sprite_to_draw = self.idle_frames[self.direction]
        
        # Flip sprite if moving left
        if self.flip_sprite and self.direction == "side":
            sprite_to_draw = pygame.transform.flip(sprite_to_draw, True, False)
        
        # Update the sprite image and rect
        self.image = pygame.transform.scale(
            sprite_to_draw,
            (int(game_state.frog_width), int(game_state.frog_height))
        )
        self.rect = self.image.get_rect(topleft=(self.pos.x, self.pos.y))
