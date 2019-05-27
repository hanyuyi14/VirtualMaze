# Raycasting guide
This utility takes eye-tracking data and generates information about the fixated surfaces or objects.
![data-generationScreenshot](/docs/images/data-generation.PNG)

## Prerequisites
**Session data file:** Session file generated by VirtualMaze (Unity) during the experiment.

**.edf file:** .edf file from the EyeLink eye-tracker.

## Steps to Use
1. Select the files in the respective text boxes
+ Select the destination folder to save the output file
+ Click ***Generate*** to start processing the files.

## Output
A CSV (Comma Seperated Value) file will be generated in the destination folder.

![data-generation-outputScreenshot](/docs/images/data-generation-output.PNG)

#### Description of Columns
1. Type of data in the row (string/text).
2. Timestamp of the gaze data used to identify the fixated object (unsigned int)
3. Name of the object fixated by the gaze, or Message received by EyeLink (string/text).

[Can we re-arrange the columns to the following order:]

*Raw Gaze Data (Pixels)*

12. gx data from the .edf file used to raycast.
13. gy data from the .edf file used to raycast.

*Subject Location in Worldspace ([Unity Units](#unity-units))*

14. X Worldspace coordinate of the subject's location in Worldspace.
15. Y Worldspace coordinate of the subject's location in Worldspace.
16. Z Worldspace coordinate of the subject's location in Worldspace.

*Gaze Location ([Unity Units](#unity-units))*

9. X Worldspace coordinate where the gaze lands.
10. Y Worldspace coordinate where the gaze lands.
11. Z Worldspace coordinate where the gaze lands.

*Gazed Object Location ([Unity Units](#unity-units))*

6. X Worldspace coordinate of the fixated object.
7. Y Worldspace coordinate of the fixated object.
8. Z Worldspace coordinate of the fixated object.

*Gaze Position in Object ([Unity Units](#unity-units))*

4. 2D X location of gaze with reference to the center of the fixated object.
5. 2D Y location of gaze with reference to the center of the fixated object.

See [Gaze Position in Object](#relative-position) for more details.

#### Unity Units
Unity Units are values Unity uses to position the various game objects in Worldspace, where the center of the floor of the maze is located at (x: 0, y: 0, z: 0). For reference and scaling, the "rooms" used in VirtualMaze are 25 by 25 Unity Units and the ceiling is 4.93 Unity Units high.

The default settings for units in Unity is 1 Unity Unit = 1 meter. However, because the scaling is mutable, feel free to scale these values as required.

#### Object Size Reference

- **Posters:** width: 2.24, height: 1.4, thickness: 0 Unity Units
- **Outer Walls:** width: 5, height: 5, thickness: 0.1 Unity Units
- **Inner Walls/Barriers:** width: 5, height: 3.11, thickness: 0.1 Unity Units
- **Ground and Ceiling:** width: 25, length: 25, thickness: 0 Unity Units
- Ceiling Height: 4.93 Unity Units
- **Cue Image:** width: 0.4, length: 0.2, thickness: 0 Unity Units
- **Hint Image** width: 0.2, length: 0.1, thickness: 0 Unity Units
- **Height of Viewport:** 1.85 Unity Units from the floor

See [Cue Image and Hint Image ](#cueImage-and-hintImage) for definition of Cue Image and Hint Image.

#### Object Name References

##### Cue Image and Hint Image
![cue-hint-image](/docs/images/cue-hint-image.png)

There are 2 kinds of cues presented to the subject, and for convenience, the larger cue is referred to as the Cue Image, while the smaller image at the top is referred to as the Hint Image.


## Gaze Position in Object
Gaze Position in Object represents the coordinates of the gaze position from the center of the object.

###### Image Cue in 3D Space
![relative position explanation](/docs/images/relativePos-explaination.png)

The center of the object (represented by the circle) is the 3D coordinates of its position in virtual space.

The point where the gaze hits the object (represented by the triangle) is also represented by its position in virtual space.

For the example shown in the image, the resultant Gaze Position in Object is **(x: 1, y: 1)** because the center of the 2D image is taken to be the origin of the 2D coordinate system.

###### Gaze point with reference to the center of the Cue Image.
![reading-relative-position](/docs/images/reading-relative-position.png)