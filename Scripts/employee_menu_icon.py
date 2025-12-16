import pygame
from game_state import game_state

class EmployeeMenuIcon(pygame.sprite.Sprite):
    """A sprite class for the employee menu icon button."""
    
    def __init__(self, icon_size=32):
        """Initialize the employee menu icon sprite.
        
        Args:
            icon_size: Size to display the icon (default 32x32)
        """
        super().__init__()
        
        self.icon_size = icon_size
        
        # Load employee menu image
        self.image = pygame.transform.scale(pygame.image.load(r"..\Assets\Icons\EmployeeMenu.png").convert_alpha(), (self.icon_size, self.icon_size))
        
        # Set initial position
        self.update_position()

    def update_position(self):
        """Update the position to bottom left corner based on game_state dimensions."""
        padding = 10
        self.rect = pygame.Rect(
            padding,
            game_state.base_height - self.icon_size - padding,
            self.icon_size,
            self.icon_size
        )
    
    def on_click(self):
        """Toggle the employee menu state when clicked."""
        game_state.toggle_employee_menu()
    
    def update(self, *args, **kwargs):
        """Update method (required by Sprite class)."""
        pass