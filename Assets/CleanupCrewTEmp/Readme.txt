-------------THE CODEBASE-----------

PLAYERS:
Player consists of the gameobject itself, a PlayerInventory, PlayerInput, PlayerController. and likely a rigidbody and CharacterController.
it will also have some fishnet specific components for syncing Transforms etc.


ITEMS:
These consist of an itemModel, RigidBody, an ItemBehaviour Script and a scriptable object which is stored inside the ItemBehaviour.

ENEMIES:
These consist of a custom Enemy_NAME_Controller.cs which implements reusable Behaviour States through the Monster.cs Base class and the 
MonsterSenses.cs class which provides data to inform the behaviour states.
Enemy Models and Animations are yet to be figured out.... maybe they will be done on a per monster basis?

EVENTS AND TRIGGERS:
Events can be activated via InteractionTrigger, CollisionTrigger, TimedTrigger.

MANAGERS:
Sound is accessible through a SoundManager singleton providing both client and global server call functions.
UI will be accessible in a simalar fashion.
other potential singletons include: InputManager, GameManager, SettingsManager, PlayerManager, SceneManager.

----------------------------------------------------------------

Implementations and other helper classes will be stored in a scripts folder away from the codebase. 
the codebase should only really contain key functionality and structure.