# RWS Monad
The RWS Monad is a composite monad in functional programming that combines the functionalities of the Reader, Writer, and State monads. It allows a function to read from a shared environment (Reader), produce a log or auxiliary output (Writer), and manage state (State) within a single monadic context. The RWS Monad encapsulates computations that require these three aspects, providing a unified interface for operations that need to access shared configuration data, accumulate logs, and modify state. This leads to more expressive and maintainable code by centralizing the management of these different concerns in a consistent and cohesive manner.

## Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.common": "https://github.com/AmarilisCreation/AmarilisLib.Common.git?path=src/Common/Assets/Plugins",
    "com.amariliscreation.rws_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/RWS/Assets/Plugins",
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/RWS/Assets/RWSMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.