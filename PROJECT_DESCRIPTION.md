# CubeTower Project Description

## Overview

`CubeTower` is a Unity project for a 2D drag-and-drop tower game.

Core gameplay:
- the bottom area contains an infinite horizontal scroll of source elements
- a player drags an element from the scroll into the right zone to build a tower
- placed tower elements can be dragged again
- dragged elements can be disposed into a hole in the left zone
- feedback text is shown above the bottom area
- tower progress is saved and restored between sessions

The project is built around Zenject dependency injection, MessagePipe event delivery, configuration via `ScriptableObject`, and object reuse via pooling.

## External Dependencies

Main packages from `Packages/manifest.json`:
- `Zenject`
- `MessagePipe`
- `MessagePipe.Zenject`
- `UniTask`
- `DOTween`
- `TextMesh Pro`
- `Unity Input System`
- `UGUI`
- `URP`

## Project Structure

Main code root:
- `Assets/GameAssets/Scripts`

Top-level modules:
- `Core`
- `Screen`
- `Scroll`
- `Input`
- `Drag`
- `Tower`
- `Hole`
- `Feedback`
- `Localization`
- `Save`
- `SceneLoading`
- `ObjectPoolManager`
- `Untilities`

## Module Breakdown

### Core

Files:
- `Core/GameInitializer.cs`

Responsibility:
- initializes screen zones after Zenject injects them

Current behavior:
- calls `Initialize()` for left, right, and scroll zones

### Screen

Files:
- `Screen/ScreenZoneBase.cs`
- `Screen/ScreenZonesInstaller.cs`
- `Screen/LeftZoneView.cs`
- `Screen/RightZoneView.cs`
- `Screen/BottomScrollZoneView.cs`
- `Screen/ScrollZoneView.cs`
- `Screen/ScreenLayoutController.cs`
- `Screen/IScreenZone.cs`

Responsibility:
- owns visual and layout representation of the three screen areas
- provides abstractions for left zone, right zone, scroll zone, hole view, and scroll view

Key points:
- `ScreenZonesInstaller` spawns zones from prefabs and binds interfaces
- `LeftZoneView` also exposes `HoleRoot`
- `ScrollZoneView` exposes both content root and `ScrollRect`

### Scroll

Files:
- `Scroll/ScrollFactoryInstaller.cs`
- `Scroll/ScrollRuntimeInstaller.cs`
- `Scroll/ScrollFeedConfig.cs`
- `Scroll/ScrollRuntimeConfig.cs`
- `Scroll/ScrollElementData.cs`
- `Scroll/ScrollElementFactory.cs`
- `Scroll/ScrollElementSpawner.cs`
- `Scroll/ScrollElementRegistry.cs`
- `Scroll/ScrollElementDataRepository.cs`
- `Scroll/BasicScrollElement.cs`
- `Scroll/ScrollElementMessages.cs`

Responsibility:
- stores source element data
- creates scroll items
- spawns initial scroll content
- manages runtime drag parameters and scroll-related animations

Key points:
- `ScrollFeedConfig` contains the source list of bottom elements
- `ScrollRuntimeConfig` contains drag conditions and scroll-related animation settings
- `ScrollElementSpawner` creates initial bottom elements on startup
- scroll elements are infinite sources and are not consumed by tower placement

### Input

Files:
- `Input/InputManager.cs`
- `Input/ScrollInputHandler.cs`
- `Input/ScrollDragSessionController.cs`
- `Input/ScrollRectMovementController.cs`
- `Input/DragSession*.cs`
- `Input/IScrollInputHandler.cs`
- `Input/IScrollDragSessionController.cs`
- `Input/IScrollMovementController.cs`

Responsibility:
- converts pointer events into drag sessions
- stops and resumes `ScrollRect` movement during drag
- publishes drag lifecycle messages

Key points:
- `InputManager` subscribes to scroll element presses and drag element presses
- `ScrollDragSessionController` is the main drag-session state machine
- drag flow is message-driven:
  - `DragSessionStartedMessage`
  - `DragSessionMovedMessage`
  - `DragSessionEndedMessage`
  - `DragSessionCancelledMessage`
  - `DragSessionPlacedMessage`
  - `DragSessionReturnedMessage`
  - `DragSessionDisposalStartedMessage`
  - `DragSessionDisposedMessage`

### Drag

Files:
- `Drag/DragFactoryInstaller.cs`
- `Drag/DragElementFactory.cs`
- `Drag/DragElementBase.cs`
- `Drag/BasicDragElement.cs`
- `Drag/IDragElement.cs`
- `Drag/IDragElementFactory.cs`
- `Drag/DragElementPressedMessage.cs`

Responsibility:
- creates runtime draggable elements
- renders drag animations
- reacts to drag lifecycle messages

Key points:
- drag elements are created through `ObjectPoolManager`
- `BasicDragElement` handles:
  - drag-start scale animation
  - start pull animation from scroll element
  - placement animation onto tower
  - cancel return animation
  - disposal animation into hole
  - delayed falling when upper tower blocks shift down

### Tower

Files:
- `Tower/TowerInstaller.cs`
- `Tower/TowerService.cs`
- `Tower/TowerState.cs`
- `Tower/TowerPositionResolver.cs`
- `Tower/TowerPlacementRuleValidator.cs`
- `Tower/FirstBlockInsideRightZoneRule.cs`
- `Tower/StackOnTopHitRule.cs`
- `Tower/HeightLimitRule.cs`
- `Tower/TowerDragDropHandler.cs`
- `Tower/TowerConfig.cs`
- `Tower/TowerActionMessage.cs`
- `Tower/TowerBlockShiftedMessage.cs`
- `Tower/TowerSnapshot*.cs`

Responsibility:
- owns tower domain state
- validates placement rules
- computes placement positions
- removes blocks and shifts upper blocks down
- emits tower feedback actions

Key points:
- `TowerState` stores current tower blocks and height-limit state
- `TowerPositionResolver` calculates first-block placement and stacked placement
- placement rules are composable and run through `TowerPlacementRuleValidator`
- `TowerDragDropHandler` receives drag end messages and decides:
  - place on tower
  - return block
  - dispose block into hole

### Hole

Files:
- `Hole/HoleInstaller.cs`
- `Hole/HoleService.cs`
- `Hole/HoleDisposalRuleValidator.cs`
- `Hole/OvalHoleHitRule.cs`
- `Hole/HoleConfig.cs`

Responsibility:
- validates whether a dragged element was dropped into the hole
- exposes disposal target position and result

Key points:
- hit testing is elliptical, not rectangular
- `OvalHoleHitRule` uses the `RectTransform` of `HoleRoot`
- gizmo rendering for the ellipse lives in `LeftZoneView`

### Feedback

Files:
- `Feedback/FeedbackInstaller.cs`
- `Feedback/FeedbackController.cs`
- `Feedback/FeedbackFactory.cs`
- `Feedback/FeedbackItemView.cs`
- `Feedback/FeedbackConfig.cs`

Responsibility:
- listens for tower action messages
- localizes text keys
- spawns pooled feedback items into a vertical layout root

Key points:
- feedback items are pooled
- each item fades in, stays visible, fades out, then returns to pool

### Localization

Files:
- `Localization/LocalizationInstaller.cs`
- `Localization/LocalizationManager.cs`
- `Localization/LocalizationConfig.cs`
- `Localization/LocalizedStringEntry.cs`
- `Localization/LocalizedTextMeshPro.cs`

Responsibility:
- provides string lookup by key
- applies localized values to UI text

Key points:
- `LocalizationConfig` stores `List<LocalizedStringEntry>`
- `LocalizationManager.GetString(key)` returns:
  - localized value if key exists
  - the key itself if not found

### Save

Files:
- `Save/GameSaveInstaller.cs`
- `Save/GameSaver.cs`
- `Save/GameSceneProgressInstaller.cs`
- `Save/GameSceneProgressHandler.cs`
- `Save/MenuSceneProgressInstaller.cs`
- `Save/MenuSceneProgressHandler.cs`
- `Save/GameSaveData.cs`
- `Save/ContinueGameButton.cs`
- `Save/NewGameButton.cs`
- `Save/ExitGameButton.cs`
- `Save/ExitToMenuButton.cs`

Responsibility:
- stores save data
- restores game progress on game scene
- saves tower progress when tower state changes
- drives menu button availability and behavior

Key points:
- save storage uses `PlayerPrefs`
- current key: `cube_game_progress`
- `GameSaver` only stores and loads `GameSaveData`
- `GameSceneProgressHandler` orchestrates:
  - restore on game scene initialization
  - save on `DragSessionPlacedMessage`
  - save on `DragSessionDisposedMessage`
- `MenuSceneProgressHandler` exposes:
  - `HasSave`
  - `StartNewGame()`

### SceneLoading

Files:
- `SceneLoading/SceneLoadingInstaller.cs`
- `SceneLoading/SceneLoader.cs`
- `SceneLoading/LoadingScreenView.cs`
- `SceneLoading/SceneLoadingConfig.cs`
- `SceneLoading/ISceneLoader.cs`

Responsibility:
- loads scenes with a loading screen
- keeps a minimum loading time
- animates loading screen fade in and fade out

Key points:
- `LoadingScreenView` is persistent through scene changes
- `SceneLoader` uses `UniTask` and `SceneManager.LoadSceneAsync`

### ObjectPoolManager

Files:
- `ObjectPoolManager/PoolManagerInstaller.cs`
- `ObjectPoolManager/PoolManager.cs`
- `ObjectPoolManager/PooledObject.cs`
- `ObjectPoolManager/PooledObjectPool.cs`
- `ObjectPoolManager/PooledObjectFactory.cs`
- `ObjectPoolManager/PoolGroup.cs`

Responsibility:
- central object pool storage and spawn/despawn API

Key points:
- `PoolGroup` defines pool id, prefab, initial size, max size, expand method
- drag elements and feedback items are pooled through this module
- despawned objects are moved under a persistent `[POOL_DESPAWNED]` root

### Untilities

Files:
- `Untilities/ButtonPressAnimation.cs`

Responsibility:
- contains isolated UI helper animation logic

## Scene Composition

### Expected Game Scene Installers

Typical game scene composition:
- `ScreenZonesInstaller`
- `ScrollFactoryInstaller`
- `ScrollRuntimeInstaller`
- `DragFactoryInstaller`
- `TowerInstaller`
- `HoleInstaller`
- `FeedbackInstaller`
- `LocalizationInstaller`
- `GameSaveInstaller`
- `GameSceneProgressInstaller`
- `SceneLoadingInstaller`
- `PoolManagerInstaller`

### Expected Menu Scene Installers

Typical menu scene composition:
- `GameSaveInstaller`
- `MenuSceneProgressInstaller`
- `SceneLoadingInstaller`
- `LocalizationInstaller`
- `PoolManagerInstaller`

## Runtime Flow

### Game Start Flow

1. Zenject installers bind services and configs.
2. `ScreenZonesInstaller` spawns the three screen zones.
3. `ScrollRuntimeInstaller` binds MessagePipe brokers and runtime services.
4. `ScrollElementSpawner` creates the initial bottom elements from `ScrollFeedConfig`.
5. `GameSceneProgressHandler` attempts to restore `GameSaveData`.
6. If save exists, `TowerService.Restore()` rebuilds the tower from snapshot data.

### Drag From Scroll To Tower

1. Player presses a bottom scroll element.
2. `InputManager` routes the press to `ScrollInputHandler`.
3. `ScrollDragSessionController` enters pending state.
4. When distance and scroll-velocity conditions are valid, a pooled drag element is created.
5. Drag lifecycle messages are published.
6. `TowerDragDropHandler` receives drag end.
7. If tower placement succeeds:
   - `TowerService` updates tower state
   - `DragSessionPlacedMessage` is published
   - feedback message is published
   - save is updated through `GameSceneProgressHandler`

### Drag From Tower To Hole

1. Player presses an existing tower block.
2. `InputManager` starts a tower-origin drag session.
3. On drag end, `TowerDragDropHandler` asks `HoleService` to validate hole hit.
4. If successful:
   - `TowerService.TryRemove()` removes the block
   - upper blocks receive `TowerBlockShiftedMessage`
   - disposal animation starts
   - save is updated

### Failed Drag

Two cases exist:
- scroll-origin drag miss:
  - temporary drag element returns or despawns
  - bottom source element view returns
- tower-origin drag miss:
  - element animates back to its original tower position
  - `BlockReturned` feedback is published

## Architectural Patterns Used

### Dependency Injection

Used through Zenject:
- installers bind services, configs, and message brokers
- views and services receive dependencies by constructor injection or `[Inject]`

### Event-Driven Architecture

Used through MessagePipe:
- input and gameplay systems communicate through messages
- visual reactions are decoupled from decision-making logic
- save and feedback modules listen to domain events instead of direct calls

### Factory Pattern

Factories used in the project:
- `ScrollElementFactory`
- `DragElementFactory`
- `FeedbackFactory`
- `PooledObjectFactory`

### Object Pool Pattern

Used for frequently reused runtime objects:
- drag elements
- feedback items

### Rule Pipeline Pattern

Used for validation:
- tower placement rules
- hole disposal rules

This makes constraints extendable without rewriting the main service.

### Service Layer

Main orchestration services:
- `TowerService`
- `HoleService`
- `GameSaver`
- `SceneLoader`
- `LocalizationManager`

### State Holder Pattern

Used through `TowerState`:
- tower block list
- top block access
- height-limit flag

## Configurations

### `ScrollFeedConfig`

Purpose:
- defines the source list of bottom scroll elements

Stored data:
- `initialElements`

### `ScrollRuntimeConfig`

Purpose:
- defines drag conditions and scroll/drag-related animations

Main groups:
- drag conditions
- drag timing
- drag start animation
- drag cancel animation
- scroll element show animation

### `TowerConfig`

Purpose:
- defines tower rules, tower animations, and localized feedback keys

Main groups:
- placement rules
- placement animation
- tower shift animation
- feedback keys

### `HoleConfig`

Purpose:
- defines disposal animation settings for hole interaction

Main groups:
- dispose animation

### `FeedbackConfig`

Purpose:
- defines lifetime and fade animation of feedback items

Main groups:
- lifetime
- fade animation

### `LocalizationConfig`

Purpose:
- stores localization entries

Stored data:
- `List<LocalizedStringEntry>`

### `SceneLoadingConfig`

Purpose:
- defines loading screen timing and fade animation

Main groups:
- loading
- fade animation

### `PoolGroup`

Purpose:
- defines pool settings for a particular pooled prefab

Stored data:
- pool id
- prefab
- initial size
- max size
- expand method

## Save Data Structure

Current save payload:
- `GameSaveData`
  - `TowerSnapshot`
    - list of `TowerSnapshotBlock`
    - `IsHeightLimitReached`

Each snapshot block stores:
- `ElementId`
- `Position`

## UI Buttons

Menu and navigation buttons:
- `ContinueGameButton`
  - active only if save exists
  - loads game scene without clearing save
- `NewGameButton`
  - clears save
  - loads game scene
- `ExitGameButton`
  - quits application
- `ExitToMenuButton`
  - saves current progress
  - loads menu scene

## Current Technical Characteristics

- drag system is implemented without physics
- tower and hole decisions are domain-driven, then reflected through animation messages
- scroll elements are infinite sources
- first tower block is validated against right-zone bounds
- hole hit detection is oval
- project uses both direct view logic and event-driven orchestration

## Notes

- The project already contains a clear split between domain-like services and Unity view classes.
- The main coupling point of the runtime is `TowerDragDropHandler`, which coordinates drag result resolution.
- The save system is intentionally separated:
  - `GameSaver` stores raw save data
  - scene handlers decide when to load or save
- Localization is key-based and intentionally tolerant to missing entries.
