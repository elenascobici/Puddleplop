import pygame

class ShopOutside(pygame.sprite.Sprite):
    """Sprite for the shop exterior."""
    def __init__(self):
        super().__init__()
        self.image = pygame.image.load(r"..\Assets\Tilemaps\ShopExterior.png").convert_alpha()
        self.rect = self.image.get_rect(topleft=(0, 0))

    def update(self, *args, **kwargs):
        pass