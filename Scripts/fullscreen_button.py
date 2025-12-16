"""
A module defining a fullscreen toggle button sprite.
"""
import pygame


class FullscreenButton(pygame.sprite.Sprite):
    """A sprite class for the fullscreen toggle button."""
    
    def __init__(self, screen_width, screen_height, icon_size=32):
        """Initialize the fullscreen button sprite.
        
        Args:
            screen_width: Width of the screen in pixels
            screen_height: Height of the screen in pixels
            icon_size: Size to display the button (default 32x32)
        """
        super().__init__()
        
        self.screen_width = screen_width
        self.screen_height = screen_height
        self.icon_size = icon_size
        
        # Create a simple fullscreen button (since no icon exists)
        self.image = pygame.Surface((icon_size, icon_size))
        self.image.fill((100, 100, 100))  # Gray background
        pygame.draw.rect(self.image, (200, 200, 200), (2, 2, icon_size - 4, icon_size - 4), 2)
        
        # Position to the left of the hoe icon
        padding = 10
        icon_spacing = 5
        self.rect = pygame.Rect(
            screen_width - (icon_size + padding) - (icon_size + icon_spacing),
            screen_height - icon_size - padding,
            icon_size,
            icon_size
        )
    
    def update(self, *args, **kwargs):
        """Update method (required by Sprite class)."""
        pass
