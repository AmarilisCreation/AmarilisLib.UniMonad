# State Monad
The State Monad is a type of monad used in functional programming to handle stateful computations in a functional way. It encapsulates a state along with a computation, allowing functions to read and modify the state without explicitly passing it through each function call. The State Monad provides a mechanism to thread state through a sequence of computations while maintaining functional purity. This results in cleaner and more modular code, as it separates state management from the core logic of the computations.

## Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.common": "https://github.com/AmarilisCreation/AmarilisLib.Common.git?path=src/Common/Assets/Plugins",
    "com.amariliscreation.state_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/State/Assets/Plugins",
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/State/Assets/StateMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.