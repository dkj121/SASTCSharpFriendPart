# SASTCsharpFriendPart

![img](/SASTCSharpFriendPart.UI/Assets/image.png)
这里有一群可爱的动物小伙伴~  
目前这个项目还是仅仅只有最基本的登录/注册界面，我们希望你能够为当前的桌面端 APP 添加供小动物们沟通交流的交流界面。（参考 朋友圈、QQ 空间、微博、Twitter（现 X ）、Discard 等均可）  
目前这个项目采用的是 WinUI3 框架，对于 MacOS 或者 Linux用户来说不够友好，如果需要，你也可以选择其他框架如 .NET MAUI、React Native、Electron、Avalonia 等均可  
注意：对于本套题目，我们最注重的部分正是 **桌面 APP**  的实现 

## （1）基础要求

- [ ] 在自己的电脑上完成数据绑定（目前本项目仅仅和 Core 中的 [data](/SASTCSharpFriendPart.Core/Data/data.json) 保持同步）
- [ ] 为当前应用搭建出一套聊天交流界面
- [ ] 为当前应用搭建出一套用户个人界面

## （2）进阶要求

对于本套题目，我们同时还搭建了一套 [远程 Service](https://github.com/dkj121/SASTCSharpFriendPartService)  用来负责信息同步，在这里，我们希望你能够完成远程数据绑定，并实现各自桌面端之间的同步  
目前这份 Service 采用的是 Minimal API，假如你对其他类型的框架更熟悉的话也可以自行选择简单重构这个项目  

- [ ] 自行部署远程服务（vercel、render、Azure Web App、AWS、阿里云、腾讯云甚至华为云均可）
- [ ] 完成与远程数据之间的通信
- [ ] 为添加一套单元测试

## （3）开放内容

1. 添加语音聊天功能！
2. 顺手尽可能美化一下你的前端
3. 添加其他你觉得可能用得到的功能

## 项目结构

```txt

SASTCsharpFriendPart
├─📂SASTCSharpFriendPart.Core // 业务代码
└─📂SASTCSharpFriendPart.UI   // UI 代码
    ├─📂Assets
    ├─📂Properties
    └─📂ViewModels

```
