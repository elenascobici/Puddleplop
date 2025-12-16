import pygame
from game_state import game_state


class Hoe(pygame.sprite.Sprite):
    """A sprite class for the hoe icon button."""
    
    def __init__(self, icon_size=32):
        """Initialize the hoe icon sprite.
        
        Args:
            icon_size: Size to display the icon (default 32x32)
        """
        super().__init__()
        
        self.icon_size = icon_size
        
        # Load hoe images
        self.hoe_normal = pygame.image.load(r"..\Assets\Icons\Hoe.png").convert_alpha()
        self.hoe_highlighted = pygame.image.load(r"..\Assets\Icons\HoeHighlighted.png").convert_alpha()
        
        # Set initial position
        self.update_position()
        
        # Set initial image
        self.update_image()
    
    def update_position(self):
        """Update the position to bottom right corner based on game_state dimensions."""
        padding = 10
        self.rect = pygame.Rect(
            game_state.base_width - self.icon_size - padding,
            game_state.base_height - self.icon_size - padding,
            self.icon_size,
            self.icon_size
        )
    
    def update_image(self):
        """Update the displayed image based on hoe state."""
        if game_state.is_hoe_active():
            base_image = self.hoe_highlighted
        else:
            base_image = self.hoe_normal
        
        self.image = pygame.transform.scale(base_image, (self.icon_size, self.icon_size))
    
    def on_click(self):
        """Toggle the hoe state when clicked."""
        game_state.toggle_hoe()
        self.update_image()
    
    def update(self, *args, **kwargs):
        """Update method (required by Sprite class)."""
        pass
