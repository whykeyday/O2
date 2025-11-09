from PIL import Image
import os

# Paths to cursor images
cursor_dir = r"C:\Users\whyke\github\O2\Assets\Images and GIFs\Misc"
cursors = ["CursorOriginalLook.png", "CursorCover.png"]

# Resize to 50% of original size (adjust this percentage as needed)
scale_factor = 0.5

for cursor_name in cursors:
    cursor_path = os.path.join(cursor_dir, cursor_name)
    
    if os.path.exists(cursor_path):
        # Open image
        img = Image.open(cursor_path)
        original_size = img.size
        
        # Calculate new size
        new_size = (int(original_size[0] * scale_factor), int(original_size[1] * scale_factor))
        
        # Resize with high quality
        img_resized = img.resize(new_size, Image.Resampling.LANCZOS)
        
        # Save back
        img_resized.save(cursor_path)
        print(f"Resized {cursor_name}: {original_size} -> {new_size}")
    else:
        print(f"File not found: {cursor_path}")

print("\nDone! Cursor images have been resized.")
