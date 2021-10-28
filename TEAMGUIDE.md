# TEAM GUIDE
It is the team's responsibility to make the TECH-D project maintainable for future iterations. The ideas, notes, and tips found in this document are essential guides not standards. 

<br>

## Developers Guide
`Author: Raul`
### Unity
- When having trouble with NavMesh,
	- NavMeshBaker is designed to handle navmesh baking at runtime. It stores its data on "NavMesh" (Scenes/Prototypes). Sometimes, it produces an error if it's overridden while Baking on the Editor. (`Nav Mesh Data` changes other than "NavMesh" which is the mesh data used at runtime) 
		- To solve this, 
			- Clear the `Nav Mesh Data` property before running or building the application, or
			- Rename the current Nav Mesh Data specifically to "NavMesh"

- When editing, balance editing on the prefab and on editor.
	- Edit on editor upon convenience and don't forget to override changes/touches in the prefab (if needed)
	- Edit on prefab to allow updates be reflected globally (for clones) 
	- Utilize override / revert function when necessary

- When adding new building models (.fbx), 
	- Tick `Read/Write Enabled ` to allow NavMeshBuilder read the models' mesh information
	- Tick `Generate Colliders` to allow Navigation's ray casting
	- Rotate models via Parent `Model` transform not on the model transform itself to allow preserve transforms whenever the model is updated
	- In the Parent Building transform, re-select the `SelectableBuilding` layer to include the newly added building model's collider be selected while at runtime

- When calibrating cameras,
	- Avoid ticking "Save on Gameplay" in CinemachineVirtualCameras. This function great during early development but can cause unintended or unforeseen results if not used carefully

- When building the project, 
	- make sure that there are no errors
	- proper version folder has been created (following [SemVer](https://semver.org/))

### Script Improvements
- Search
	- Re-implement or re-write search script. Current version relies implementation of Unity-UI-Extensions which needs further modification for better Search feature
- Refactor UIManager.cs into separate scripts
	- Currently it's GIGANTIC and is responsible with mostly all the routines for MainCanvas. Delegate separate routine concerns Building, Search, Button instatiation, etc.
- Flexible Resolutions
	- Current resolution support is 16:9 ratio
- Road Architect System 
	- It is fragile and unstable in the current version, curse my incompetence jk. It is recommended to find a simpler asset like PathCreator by Sebastian Lague


## Modelers Guide
#### Hans
- Shift + Tab for object snapping, very good for placements as it snaps into an edge of a polygon.

- Measure tool works well with object snap if you want to measure the height and width of the object.

- When putting colors on a polygon, look for material properties, look at the list and find the plus button beside it, click it, press new below list, and when the use nodes button is blue press it to make it grey(idk what it does but it works for me) and select the designated color you want a certain surface of a polyygon to be.

- During measure tool,hold ctrl while finding an edge in edit mode to make sure you snap an edge correctly.

- Loop cut makes a center line that is halved by the length or width edge you choose.

- Holding ctrl while extending will increment the extend in 1 digit inches which avoids making your extensions have decimals. (i.e., target:8 inches extend without holding ctrl: 8.156 or 8.452 inches with holding ctrl: 8.000 inches).

- Pressing 1,2,3 makes it easy to switch between selecting vertices, edges, and faces in edit mode
	
#### Sam

Note:
> X is `red`, Y is `green`. Right hand coordinates, Z is up and `blue`.

- When using a mouse buttons
	Scrollwheel - A mini 1-axis track ball on your mouse! Brilliant!

	SWHEEL - Scales time line.

	[C]-SWHEEL - Pans the time line.

	[A]-SWHEEL - "Scrubs" through time line, i.e. repositions current frame.

	(Middle Mouse Button) rotate (orbit) view. In draw mode, accepts drawn shapes.

	[C]-MMB - scale view (zoom/move view camera closer)

	[S]-MMB - pan view (translate), reposition view in display or, as I like to think of it, "shift" the view

	[S][C]-MMB - Pan dolly a kind of zoom along your view

	[A]-MMB - Center on mouse cursor. Drag to change among constrained ortho views.

	Knife tool. Hold [C] to snap (e.g. to mid points). Snake hook (SculptMode).

- When using a knife tool,
	The Knife tool can be used to interactively subdivide (cut up) geometry by drawing lines or closed loops to create holes.

- Objects can be dragged around and placed in different hierarchical arrangements in the "Outliner". I’ve had this sometimes get stuck and it’s pretty strange, but reloading can cure it.

- Use [A]-m to merge vertices to a single point!

- When making a round,
	- Create the pipe’s path with Add → Curve → Bezier. 
	- Then go to "Object Data Properties" whose icon should look like a curve and is right above the Materials icon. 
	- Open up the "Bevel" and choose "Round". You can adjust the "Depth" setting to change the thickness of the pipe;
	(But there is also a cylinder to use it as a round or for a pipe but you need to get its perfect round measurement.

#### Ashley
Log 09-07-21: - Instead of a rough sketch and an image reference, learn to utilize accurate measurement.
- To flip, rotate, mirror, and invert mesh, use transform.

Log 09-10-21 Log:
- Concentrate on one model. (Working on one model at a time)
- Remove any unnecessary mesh, lights, or text from the scene.

Log 09-11-21 Log:
- Pay attention to the minor details when dealing with large projects.
- When working with little projects, pay attention to the smallest details.
- Develop a basic understanding of the modifier properties.

Log 09-14-21: - Modeling a quadrilateral can be difficult; learn to model it by breaking it into four sections rather than as a whole.
- Before modeling an existing structure, try to look at every visible detail and figure out where you should spend your effort.

Log 09-29-21: - Use mesh minimally and learn to model efficiently.
- Remove all materials and only apply textures after everything else has been completed.
- Acquire familiarity with the materials property.
- Using "recalculating normals" (A -> Shift N) rather than "joining all mesh" only, might sometimes destroy the model.

Log 2021-5: - Don't rush your art; keep editing even if it's only a few minutes.
- Don't stop on your momentum.
- Continue to learn from others.
- Always experiment with the tools available in Blender.
- Experiment, experiment, and then experiment some more.
- Understand and remember why you're doing this.


## Project Management
`Author: Paul`
Project Manager / Tester / Document Regulator Notes
For managing:
- Trello - to easily track the submission of version uploads, building uploads, and papers.
- Gantt - for organize employing of task for each member, also to record the time documenting the applicaiton.

***Make sure to keep track of the overall development of the application***


For testing:
- Manually - Test every button, the goal is to find defects or error in application.
	 (Intergration Manual Testing)
	 (Manual testing)
	 
 - Software Testing tools - I suggest that learning one could benefit the team in terms of quality assurance.

> NOTE: THE `GOAL` IS TO FIND `ERROR`

For documents:
- We've using the agile software development with scrum. As for this, we just used what we're comfortable with so we chose this software development approach. Also the approach was appropriate considering the given time we had during the development of the application.

GL! GL! GL!

