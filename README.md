# [视频介绍](https://www.bilibili.com/video/BV1HW411d7DM)

# Unity版本

很久之前的项目了，当时使用的是`Unity 2018.1.0`，并且我测试了几个版本，只有在这个版本的Unity上才能正常的运行。

# 使用方法

直接在Unity3D中导入整个文件夹，Unity会自动补全其他的文件。

正常打开项目后，在`Assets-Scenes`文件夹下面有一个场景`StartLevel`双击打开场景就可以启动游戏了。

# 核心算法

通过计算出每个格子中心点的坐标，让怪物沿着已经设定好的路径点通过`Translat`进行移动，如果当怪物离某个路径点的距离小于某值，则让它朝着下一个点进行移动，如果已经达到终点，则将怪物的渲染关闭。搜寻怪物逻辑即将所有怪物放在一个数组中，先遍历这个数组，然后判断怪物是否在塔的攻击范围中，如果在塔的攻击范围中，则将炮塔的目标设置为当前怪物。

# 对象池

unity生成和销毁实例跟前端生成`dom`一样，是十分消耗性能的，所以这就涉及到实例的复用。意思就是当怪物“死亡”时，其实不进行销毁，而仅仅是将怪物的渲染关闭，如果下次需要使用，则对这些事例重新进行初始化。就跟前端将`display`设置为`none`一样。

整个项目的实现难度并不难，但是里面包含的点非常多，包括UI，动画，对象池，数据的本地存取等，很适合作为初学者项目。