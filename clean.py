from pathlib import Path
import shutil

p = Path(__file__).parent

bins = p.glob('**/bin')
objs = p.glob('**/obj')

for d in bins:
    shutil.rmtree(d)
    print(f"Removed {d}")

for d in objs:
    shutil.rmtree(d)
    print(f"Removed {d}")