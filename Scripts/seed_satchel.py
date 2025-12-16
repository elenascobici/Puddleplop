import pygame
from game_state import game_state


class SeedSatchel(pygame.sprite.Sprite):
    """A sprite class for the seed satchel icon button."""
    
    def __init__(self, icon_size=32):
        """Initialize the seed satchel icon sprite.
        
        Args:
            icon_size: Size to display the icon (default 32x32)
        """
        super().__init__()
        
        self.icon_size = icon_size
        
        # Load seed satchel images
        self.satchel_normal = pygame.image.load(r"..\Assets\Icons\SeedSatchel.png").convert_alpha()
        self.satchel_highlighted = pygame.image.load(r"..\Assets\Icons\SeedSatchelHighlighted.png").convert_alpha()
        
        # Set initial position
        self.update_position()
        
        # Set initial image
        self.update_image()
    
    def update_position(self):
        """Update the position to bottom right corner, left of the hoe."""
        padding = 10
        hoe_x = game_state.base_width - self.icon_size - padding
        self.rect = pygame.Rect(
            hoe_x - self.icon_size - padding,
            game_state.base_height - self.icon_size - padding,
            self.icon_size,
            self.icon_size
        )
    
    def update_image(self):
        """Update the displayed image based on satchel state."""
        if game_state.is_satchel_active():
            base_image = self.satchel_highlighted
        else:
            base_image = self.satchel_normal
        
        self.image = pygame.transform.scale(base_image, (self.icon_size, self.icon_size))
    
    def on_click(self):
        """Toggle the satchel state when clicked."""
        game_state.toggle_satchel()
        self.update_image()
    
    def update(self, *args, **kwargs):
        """Update method (required by Sprite class)."""
        pass
