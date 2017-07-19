MR TOOLKIT README v.0.01:
=========================

Example Scene:
Check out the example scene under Hotmax MR Toolkit.
It should have a functioning MR Manager, the customized player prefab, and a third controller object (if you need it).

Initial Setup:
First step is hitting play and finding the correct indexes for each of your controllers in your scene.
The left and right controllers are in [camera rig], and the third controller is on the main level.
Adjust the indexes on the tracked object component until they are correct.
Remember their respective indexes, hit the play button to stop the scene from running, and set the indexes again.

Two Controller Setup:
If you aren't using a third controller you'll have to
1. disable the Third Controller on the scene, and
2. on the MR Manager component (on the MR Toolkit gameobject), drag in one of your two normal controllers into the Tracked Object slot

Camera Configuration:
The Zed configurations (like video quality, and greenscreen setup) are nested under the Zed Offset gameobject.
The Zed camera has a postprocessing component. This has color grading, bloom, SSAO, and other effects. You can change these by opening the ZedPost file in Hotmax MR Toolkit, in the Assets folder.


Calibration:
Calibration tools are on by default.
In this beta version you will see a white "pill" in the scene, which by grabbing with the lower grip buttons on the vive, you will be able to move the Zed Camera in the scene (represented by a multi-colored cross).
The goal is the move the calibration cross into the correct position, relative to the tracked object model.
There is a monitor in the scene that you can move with the grip buttons, this will help you align the zed camera with the virtual scene.
You can use your touchpad to select a constraint for exact movement, click the touchpad on the selection you want. For instance [Px] will constrain the camera on the Position-X axis, [Ry] is Y-Axis Rotation.
Select the Unity Button on the radial dial to go back to free-move mode.
When it looks like it's in a good position, you can hit the trigger button on your controller to save the configuration. You can also hit the Save button the MR Manager component.
Turn off the calibration tools on the MR Manager component when you're finished.


Creating a new Scene:
In this beta version the best way to create a new scene is to simply update the example scene given, with a new environment.
In Unity you can have multiple scenes open in the hierarchy at once, you can use this to have the example scene open and another scene you want the environment from. Copy the environment from one scene and past it into the example scene.
In future versions the tools and player will be prefabs you can drag into an existing scene.

*Bug Fixes made to non-Hotmax SDK's:
**Note: All classes that are non-Hotmax SDK's that have been changed will be put in the Hotmax_MR\Dependency_Overrides folder so that they are version controlled. When importing in a new dependency make sure you un-check then in the import module.
- NVRPhysicalController [line 26]: Added components [ typeof(Canvas), typeof(CanvasRenderer), typeof(RadialMenu), typeof(FlareLayer), typeof(Camera) ] to "keepTypes[]" of things to not remove during NVRHands initialization.
- TextureOverlay [line 58]: c.cullingMask &= ~(1 << 20)
