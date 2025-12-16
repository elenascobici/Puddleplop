from enum import Enum

class DragAction(Enum):
    TO_SOIL = 1
    TO_GRASS = 2

class Scenes(Enum):
    OUTSIDE = 1
    SHOP = 2

def extract_frames(sheet, rows, cols, total_frames, frame_width, frame_height, start_x=0, start_y=0):
    """
    Extract all the frames from a given spritesheet.

    @param sheet: The spritesheet surface
    @param rows: Number of rows in the spritesheet
    @param cols: Number of columns in the spritesheet
    @param total_frames: Total number of frames to extract
    @param frame_width: Width of each frame in pixels
    @param frame_height: Height of each frame in pixels
    @param start_x: Starting row index (default 0)
    @param start_y: Starting column index (default 0)
    """
    frames = []
    for row in range(start_x, rows):
        for col in range(start_y, cols):
            x = col * frame_width
            y = row * frame_height
            frame = sheet.subsurface((x, y, frame_width, frame_height))
            frames.append(frame)
            if len(frames) >= total_frames:
                return frames
    return frames