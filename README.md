# X Marks the Spot
*3D treasure-seeking adventure game made in Unity 6.1*
## Overview

X Marks the Spot was my first game developed in Unity in 2022-2023 (Updated in 2025). The project served as a way to demonstrate my ability to create a polished, optimized game experience. The game utilizes various Unity technologies, such as occlusion culling and LOD (Level of Detail), to ensure smooth performance and a visually appealing experience.

The game is an adventure where the player embarks on a quest to find a buried treasure on a mysterious island. Along the way, the player will encounter obstacles, collect gems while climbing the mountain, and navigate a perilous tunnel filled with traps. The ultimate goal is to find and unearth the treasure hidden at the top.

<img src="https://github.com/yousefalshaikh17/X-Marks-the-Spot/blob/main/thumbnail.png" width="50%">

## Development & Features

Developed using **Unity 6.1 (6000.1.3f1)** and **C# 9.0**. Key milestones included:

- Implemented core game mechanics: climbing, jumping, obstacle interaction.
- Designed multi-environment levels including mountainous terrain and trap-filled tunnels.
- Optimized performance using occlusion culling, Level of Detail (LOD), and projectile lifecycle animations.
- Implemented full controller support featuring context-sensitive input glyphs and event-driven input handling.
- Settings panel including difficulty settings.
- Created tools for asset and terrain tree management.
- Automated build pipelines with GitHub Actions.

## Installation

There are two approaches to installation. Downloading the build or downloading the source.

### Download Build

To download the latest build for the game, download it from the [releases page](https://github.com/yousefalshaikh17/x-marks-the-spot/releases/tag/latest). Windows 64 & Linux 64 are the only supported platforms at the moment. These releases are generated through GitHub actions workflows, but are manually reviewed before publication.

### Installing from source

1. Clone or fork the repository. Clone command:
```
git clone https://github.com/yousefalshaikh17/x-marks-the-spot.git
```
2. Open Unity Hub and load the project. **It is recommended to use Unity 6000.1.3f1.**
3. Unity will load all essential assets. Afterwards, you are able to modify the project or make a build yourself.

## Skills Demonstrated

This project provided valuable experience in game development and tools programming. It also gave me hands-on experience creating interactive environments and deepened my understanding of gameplay mechanics. Additionally, it reinforced my ability to develop polished, well-optimized games that deliver a smooth user experience.

- Proficiency with Unity’s input system and event-based input architecture.
- In-depth knowledge of performance optimization strategies in Unity.
- Strong gameplay systems design and interactive environment creation.
- Tools programming and workflow automation experience.
- Solving complex input device challenges, including simultaneous controller inputs and seamless device switching.
- Shader and material customization for improved visual quality.

## Gameplay Trailer

https://github.com/user-attachments/assets/fa10cadd-ae9e-4d43-9b2d-b75dc0b1fc83

Alternatively, the trailer is also available on [YouTube](https://www.youtube.com/watch?v=Gw-_fH8bKYc).

## Challenges & Learnings
The biggest challenge I faced during this project was manually altering the heightmap-based terrain. After sculpting the landscape, I realized that I made it very low. This was a problem since I planned on adding multiple islands. The issue was rectified through manual modification of the heightmap. 

## Credits
The industry is full of talented individuals and it is important to credit them properly. Credits to used assets can be found [here](/CREDITS.md).