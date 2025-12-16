import pygame
from game_state import game_state

class ShopDoor(pygame.sprite.Sprite):
    """Sprite for the door leading outside from the shop."""
    def __init__(self):
        super().__init__()
        self.image = pygame.image.load(r"..\Assets\Tiles\ShopInterior\ShopDoor.png").convert_alpha()
        self.rect = self.image.get_rect(
            bottomright=(game_state.camera_width, game_state.camera_height)
        )

    def update(self, *args, **kwargs):
        pass