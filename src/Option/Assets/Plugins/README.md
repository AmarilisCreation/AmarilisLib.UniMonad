# Option Monad
The Option Monad, also known as the Maybe Monad in some programming languages, is a type of monad used to represent computations that might fail or return no value. It encapsulates an optional value; the computation can result in either a value (Just) or no value (None). The Option Monad allows for chaining operations on optional values without needing to check explicitly for the presence or absence of a value at each step. This leads to more concise and readable code when handling potentially missing values.

## Install
### Using UnityPackageManager
Find the manifest.json file in the Packages folder of your project and edit it to look like this.
```
"scopedRegistries": [
  "dependencies": {
    "com.amariliscreation.option_monad": "https://github.com/AmarilisCreation/AmarilisLib.UniMonad.git?path=src/Option/Assets/Plugins",
  ...
  }
```

## Example
We have prepared [sample code](https://github.com/AmarilisCreation/AmarilisLib.UniMonad/tree/master/src/Option/Assets/OptionMonadExample.cs), so please take a look there.

## License
This library is under the MIT License.