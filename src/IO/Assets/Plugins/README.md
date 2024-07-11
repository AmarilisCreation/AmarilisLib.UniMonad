# IO Monad
The IO Monad is a type of monad used in functional programming to handle input/output operations in a pure functional way. Since pure functions cannot have side effects, the IO Monad provides a way to describe and sequence side effects while maintaining functional purity. The IO Monad encapsulates actions that interact with the external world (e.g., reading a file, printing to the console) and allows these actions to be composed and sequenced within the monadic framework. This ensures that side effects are controlled and predictable, making it easier to reason about and maintain the code.

## Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.io_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/IO/Assets/Plugins",
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/IO/Assets/IOMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.