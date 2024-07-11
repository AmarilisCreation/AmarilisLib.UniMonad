# Try Monad
The Try Monad is a type of monad used in functional programming to handle computations that may throw exceptions. It encapsulates a computation that can either result in a successful value or an exception, providing a way to handle errors without using traditional try-catch blocks. The Try Monad has two primary states: Success, which contains the successfully computed value, and Failure, which contains the exception. This allows for chaining operations while propagating errors in a controlled manner, leading to cleaner and more maintainable error handling in code.

## Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.try_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Try/Assets/Plugins",
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Try/Assets/TryMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.