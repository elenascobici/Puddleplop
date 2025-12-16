"""
A module for saving and loading player data in JSON format.
"""
import json
import os

SAVE_FILE = "save.json"


def load():
    """Load player data from save.json.
    
    Returns:
        dict: The player data dictionary containing coins, level, etc.
              Returns a default dictionary if the file doesn't exist.
    """
    if os.path.exists(SAVE_FILE):
        try:
            with open(SAVE_FILE, "r") as f:
                return json.load(f)
        except (json.JSONDecodeError, IOError):
            # Return default data if file is corrupted or unreadable
            return {"coins": 0, "level": 1}
    else:
        # Return default data if file doesn't exist
        return {"coins": 0, "level": 1}


def save(data):
    """Save player data to save.json.
    
    Args:
        data (dict): The player data dictionary to save.
    """
    try:
        with open(SAVE_FILE, "w") as f:
            json.dump(data, f, indent=2)
    except IOError as e:
        print(f"Error saving file: {e}")
