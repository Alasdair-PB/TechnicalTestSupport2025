# TechnicalTestSupport2025

**Project Setup**
Developed this project using 6000.0.57f1 and packages: URP and Input System to fix missing references. 
FindAction("Cancel"); caused null references in playmode so I Assigned the InputSystem as the project wide input system and created new Input mapping for 'Boost' action 
TextMeshPro outline shaders were still missing => Window->TextMeshPro->Import TMP Essential Resources to include missing shaders 
Assigned hinge joint references to carriages as these were also missing on import
ReadMe.Asset in the project had Missing Mono script -> opened the file in an IDE to check if this ReadMe might be important however it was just a generic readme for the URP => Removed it from the project

------------------------------------------------------------------

** Notes on AI and LLMs **

No LLMs were used to generate any code that appears in this project. ChatGPT was used a couple of times for basic documentation queries where a LLM was more pratcical than search results. 

sample:
"Whats the syntax for assembly variables in Unity. i.e constants in the ide that ignores code when set to false (uses # syntax in cpp)?"
