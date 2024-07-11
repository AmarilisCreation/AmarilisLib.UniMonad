# Either Monad
The Either Monad is a type of monad used in functional programming to handle computations that may result in two different types of values, typically representing success or failure. The Either Monad encapsulates a value that can be either a "Left" or a "Right". By convention, "Left" is used to represent an error or failure, while "Right" represents a successful computation. This allows for chaining operations where each step can handle potential errors gracefully without using exceptions. The Either Monad simplifies error handling and makes the code more expressive and easier to maintain.

## Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.either_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Either/Assets/Plugins",
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Either/Assets/EitherMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.