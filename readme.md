# *BeatSaver* Master's Thesis

This repository contains all files and sources necessary for conducting a study on how changes in musical richness may affect motivation during VR exergaming.

## Important notes
Due to licensing, several Assets were removed which are required for compiling the project. Each can be found in the Unity Asset Store 

* Sirenix Odin Inspector and Serializer
* Polygon Scifi City Asset pack
* Oculus Integration

Also, it is recommended to install the following to ease development:

* Console Pro
* MonKey Commander
* Rainbow Assets
* Enhanced Hierachy

# How to use

This runs smoothly on the OculusQuest 2 system which is the recommended way of playing. Install `Builds/BeatSaver.apk` to the device and run it. You will be prompted to choose a participant ID, experimental condition, and check whether the backend is available on the local network (using the docker container present in `express-backend/`). The latter step is not required, as data is saved to the device under `Quest 2/Internal Memory/Android/data/com.MatthiasQuass.Masterarbeit.BeatSaver/files/Sessions/$DATE$/`.

If intended to run via the UnityEditor, open the project located under `Unity/`, and run `Assets/Scenes/Setup.unity`, which will open the setup screen and allows for selection of the experimental condition. Data will be stored under `C:\Users\$USERNAME$\AppData\LocalLow\Matthias Quass\BeatSaver\Sessions\$DATE$\`.

## Using the backend

Using the backend is not required for running the game or storing data. However, it is more convenient than reading from the Quest's local storage. Some software is required for running (e.g. docker, compose, yarn,...; see `express-backend/readme.md`).

# License (MIT)
Copyright 2021 Matthias Quaß

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
