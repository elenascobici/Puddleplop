import pygame


class CoinBar(pygame.sprite.Sprite):
    """A sprite class for the coin bar display."""
    
    def __init__(self, screen_width, coins=0, font_size=24):
        """Initialize the coin bar sprite.
        
        Args:
            screen_width: Width of the screen in pixels
            coins: Current number of coins
            font_size: Size of the font for text
        """
        super().__init__()
        
        self.screen_width = screen_width
        self.coins = coins
        self.font_size = font_size
        
        # Load the coin bar background image
        self.bar_image = pygame.image.load(r"..\Assets\Icons\CoinBar.png").convert_alpha()
        
        # Create font for text
        self.font = pygame.font.Font(None, font_size)
        
        # Position in top left with padding - scale to 2x size
        padding = 10
        bar_width = self.bar_image.get_width() * 2
        bar_height = self.bar_image.get_height() * 2
        self.rect = pygame.Rect(padding, padding, bar_width, bar_height)
        
        # Update the image
        self.update_image()
    
    def update_image(self):
        """Update the displayed image with current coin count."""
        # Scale the bar image to 2x size
        scaled_bar = pygame.transform.scale(self.bar_image, (int(self.rect.width), int(self.rect.height)))
        self.image = scaled_bar.copy()
        
        # Render the coin count text
        coin_text = self.font.render(str(self.coins), True, (255, 255, 255))
        
        # Position text on the right side of the bar with padding
        text_x = self.rect.width - coin_text.get_width() - 16
        text_y = (self.rect.height - coin_text.get_height()) // 2 + 1
        
        self.image.blit(coin_text, (text_x, text_y))
    
    def set_coins(self, coins):
        """Update the coin count."""
        self.coins = coins
        self.update_image()
    
    def update(self, *args, **kwargs):
        """Update method (required by Sprite class)."""
        pass


class XpBar(pygame.sprite.Sprite):
    """A sprite class for the XP/level bar display."""
    
    def __init__(self, screen_width, level=1, font_size=24):
        """Initialize the XP bar sprite.
        
        Args:
            screen_width: Width of the screen in pixels
            level: Current player level
            font_size: Size of the font for text
        """
        super().__init__()
        
        self.screen_width = screen_width
        self.level = level
        self.font_size = font_size
        
        # Load the XP bar background image
        self.bar_image = pygame.image.load(r"..\Assets\Icons\XpBar.png").convert_alpha()
        
        # Create font for text
        self.font = pygame.font.Font(None, font_size)
        
        # Position in top left, below the coin bar - scale to 2x size
        padding = 10
        coin_bar_height = pygame.image.load(r"..\Assets\Icons\CoinBar.png").get_height() * 2
        spacing = 10
        bar_width = self.bar_image.get_width() * 2
        bar_height = self.bar_image.get_height() * 2
        self.rect = pygame.Rect(padding, padding + coin_bar_height + spacing, bar_width, bar_height)
        
        # Update the image
        self.update_image()
    
    def update_image(self):
        """Update the displayed image with current level."""
        # Scale the bar image to 2x size
        scaled_bar = pygame.transform.scale(self.bar_image, (int(self.rect.width), int(self.rect.height)))
        self.image = scaled_bar.copy()
        
        # Render the level text (without "Lvl" prefix)
        level_text = self.font.render(str(self.level), True, (255, 255, 255))
        
        # Position text on the left side of the bar with padding
        text_x = 12
        text_y = (self.rect.height - level_text.get_height()) // 2
        
        self.image.blit(level_text, (text_x, text_y))
    
    def set_level(self, level):
        """Update the level."""
        self.level = level
        self.update_image()
    
    def update(self, *args, **kwargs):
        """Update method (required by Sprite class)."""
        pass
