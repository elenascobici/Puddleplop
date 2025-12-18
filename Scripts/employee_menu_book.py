import pygame
from shared import extract_frames
from game_state import game_state

class EmployeeMenuBook(pygame.sprite.Sprite):
    """A sprite class for the employee menu book"""

    SLIDE_IN = 0
    OPENING = 1
    OPEN_IDLE = 2
    CLOSING = 3
    SLIDE_OUT = 4

    def __init__(self):
        super().__init__()

        screen_height = game_state.base_height
        self.start_y = screen_height

        # Load spritesheet + frames
        book_sheet = pygame.image.load(
            r"..\Assets\Objects\BookOpenAnimationSpritesheet.png"
        ).convert_alpha()

        self.frames = [
            pygame.transform.scale(
                frame,
                (
                    int(frame.get_width() * game_state.scale_x * 1.5),
                    int(frame.get_height() * game_state.scale_y * 1.5)
                )
            )
            for frame in extract_frames(book_sheet, 1, 36, 36, 192, 128)
        ]

        self.image = self.frames[0]
        self.rect = self.image.get_rect()
        target_y = (screen_height - self.rect.height) // 2  # Center vertically

        # Animation state
        self.state = self.SLIDE_IN
        self.frame_index = 0
        self.animation_speed = 60  # frames per second
        self.frame_timer = 0

        # Initial image
        self.image = self.frames[0]
        self.rect = self.image.get_rect()

        # Positioning
        self.target_y = target_y
        self.rect.centerx = game_state.base_width // 2  # center horizontally (adjust as needed)
        self.rect.top = screen_height  # start off-screen bottom

        # Slide motion
        self.slide_speed = 500  # pixels per second

    def update(self, dt):
        if self.state == self.SLIDE_IN:
            self._slide_in(dt)

        elif self.state == self.OPENING:
            self._play_open_animation(dt)

        elif self.state == self.OPEN_IDLE:
            if not game_state.employee_menu_open:
                self.start_closing()

        elif self.state == self.CLOSING:
            self._play_closing_animation(dt)

        elif self.state == self.SLIDE_OUT:
            self._slide_out(dt)


    def _slide_in(self, dt):
        """Slide the closed book upward onto the screen."""
        self.rect.y -= self.slide_speed * dt

        if self.rect.y <= self.target_y:
            self.rect.y = self.target_y
            self.state = self.OPENING
            self.frame_index = 0
            self.frame_timer = 0

    def _play_open_animation(self, dt):
        self.frame_timer += dt

        if self.frame_timer >= 1 / self.animation_speed:
            self.frame_timer = 0
            self.frame_index += 1

            if self.frame_index >= len(self.frames):
                self.frame_index = len(self.frames) - 1
                self.state = self.OPEN_IDLE

            self.image = self.frames[self.frame_index]

    def start_closing(self):
        self.state = self.CLOSING
        self.frame_index = len(self.frames) - 1
        self.frame_timer = 0
        self.image = self.frames[self.frame_index]

    def _play_closing_animation(self, dt):
        self.frame_timer += dt

        if self.frame_timer >= 1 / self.animation_speed:
            self.frame_timer = 0
            self.frame_index -= 1

            if self.frame_index <= 0:
                self.frame_index = 0
                self.state = self.SLIDE_OUT

            self.image = self.frames[self.frame_index]

    def _slide_out(self, dt):
        """Slide the book downward off the screen."""
        self.rect.y += self.slide_speed * dt

        if self.rect.top >= game_state.base_height:
            self.kill()  # Remove the sprite from all groups

    def reset(self):
        self.state = self.SLIDE_IN
        self.frame_index = 0
        self.frame_timer = 0
        self.image = self.frames[0]
        self.rect.top = self.start_y
