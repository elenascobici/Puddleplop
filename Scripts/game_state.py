"""
Shared game state module for managing global game variables.
"""

from shared import Scenes

class GameState:
    """Manages shared state variables across different game modules."""

    def __init__(self):
        """Initialize game state."""
        # Tool states
        self.hoe_on = False
        self.satchel_on = False
        
        # Drag state
        self.is_dragging = False
        self.drag_action = None  # TO_SOIL or TO_GRASS

        # Employee menu state
        self.employee_menu_open = False
        
        # Screen configuration
        self.base_width = 1280
        self.base_height = 720
        self.camera_width = 400
        self.camera_height = 240
        self.world_width = 480
        self.world_height = 240
        self.tile_size = 16
        self.num_tiles_x = self.world_width // self.tile_size
        self.num_tiles_y = self.world_height // self.tile_size
        self.total_tiles = self.num_tiles_x * self.num_tiles_y

        # Player info
        self.frog_width, self.frog_height = 16, 16

        # Scene state
        self.current_scene = Scenes.OUTSIDE
    
    def set_base_size(self, base_width, base_height):
        """Update the base game dimensions.
        
        Args:
            base_width: Width of the base game surface in pixels
            base_height: Height of the base game surface in pixels
        """
        self.base_width = base_width
        self.base_height = base_height
    
    def toggle_hoe(self):
        """Toggle the hoe state on/off."""
        self.hoe_on = not self.hoe_on
        # Turn off satchel when hoe is toggled on
        if self.hoe_on:
            self.satchel_on = False
    
    def set_hoe(self, state):
        """Set the hoe state to a specific value."""
        self.hoe_on = state
    
    def is_hoe_active(self):
        """Check if the hoe is currently active."""
        return self.hoe_on
    
    def toggle_satchel(self):
        """Toggle the satchel state on/off."""
        self.satchel_on = not self.satchel_on
        # Turn off hoe when satchel is toggled on
        if self.satchel_on:
            self.hoe_on = False
    
    def set_satchel(self, state):
        """Set the satchel state to a specific value."""
        self.satchel_on = state
    
    def is_satchel_active(self):
        """Check if the satchel is currently active."""
        return self.satchel_on
    
    def start_drag(self, action):
        """Start a drag operation.
        
        Args:
            action: TO_SOIL to convert tiles to soil, TO_GRASS to convert to grass
        """
        self.is_dragging = True
        self.drag_action = action
    
    def end_drag(self):
        """End a drag operation."""
        self.is_dragging = False
        self.drag_action = None
    
    def is_dragging_hoe(self):
        """Check if currently dragging the hoe."""
        return self.is_dragging and self.hoe_on
    
    def get_drag_action(self):
        """Get the current drag action."""
        return self.drag_action
    
    def set_scene(self, scene):
        """Set the current scene.
        
        Args:
            scene: Scene enum value
        """
        self.current_scene = scene
        if scene == Scenes.OUTSIDE:
            self.camera_width = 400
            self.camera_height = 240
            self.world_width = 480
            self.world_height = 240
        elif scene == Scenes.SHOP:
            self.camera_width = 400
            self.camera_height = 240
            self.world_width = 400
            self.world_height = 240
            
    def toggle_employee_menu(self):
        """Placeholder for toggling employee menu state."""
        # Implementation would go here
        if self.employee_menu_open == False:
            self.employee_menu_open = True
        else:
            self.employee_menu_open = False


# Global game state instance
game_state = GameState()
