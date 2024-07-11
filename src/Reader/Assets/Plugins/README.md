# Reader Monad
The Reader Monad, also known as the Environment Monad, is a type of monad used in functional programming to handle read-only shared environments or configurations. It allows functions to access shared data without passing it explicitly through each function call. The Reader Monad encapsulates a computation that depends on an external environment, providing a way to inject this environment into the computation chain. This leads to cleaner and more maintainable code by avoiding the need to pass the environment explicitly and by enabling better separation of concerns.

## Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.reader_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Reader/Assets/Plugins",
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Reader/Assets/ReaderMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.