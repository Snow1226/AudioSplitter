# AudioSplitter
A mod that streams BeatSaber sounds to another device/channel as well.  
Using [NAudio (https://github.com/naudio/NAudio)](https://github.com/naudio/NAudio) Library  
Currently, only ASIO is supported.  
Since WASAPI cannot run on Unity, there are no plans to support it.  
XAudio2 is under consideration.  

# Supported game versions
BeatSaber 1.34.2  

# Requirements  
BSIPA 4.3.2  
BSML (BeatSaberMarkupLanguage) 1.8.1   

# Latest version Download
The latest version can be downloaded from the following.  
[Release Page](https://github.com/Snow1226/AudioSplitter/releases)  

# Setting
![Setting.png](https://github.com/Snow1226/AudioSplitter/blob/master/Image/Setting.png)  

## Default Device Output
When enabled, sound is output from both the original output device and the ASIO device.  
When disabled, no sound will be heard from the original output device.  

## ASIO Device
Please select the ASIO device you want to use.  

## ASIO Output Channel
Please select the ASIO output Channel you want to use.
