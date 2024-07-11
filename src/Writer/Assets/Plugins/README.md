# Writer Monad
The Writer Monad is a type of monad used in functional programming to handle computations that produce a log or auxiliary output along with the main result. It allows functions to output additional data, such as logs or debug information, without modifying the primary logic of the computation. The Writer Monad encapsulates a computation paired with a monoidal value, usually representing the log, and provides operations to combine logs and results. This leads to cleaner code by separating the concerns of computation and logging, and it ensures that the logging context is managed consistently throughout the computation.

## Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.common": "https://github.com/AmarilisCreation/AmarilisLib.Common.git?path=src/Common/Assets/Plugins",
    "com.amariliscreation.writer_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Writer/Assets/Plugins",
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Writer/Assets/WriterMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.