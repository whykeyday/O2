# 给所有按钮添加Cursor Hover效果的步骤

光标图片已经缩小到50%大小。现在需要给所有按钮添加cursor hover效果。

## 需要添加的按钮列表：

### Title场景：
- ✅ StartButton (已有)
- ✅ CreditsButton (已有)

### Credits场景：
- BackButton

### Main场景：
- SunButton (图片)
- FireButton (图片)
- HumanButton (图片)
- BicycleButton (图片)

### 其他场景：
- 所有场景的 BackButton
- 所有场景的 ContinueButton
- Ending场景的 ReturnToTitleButton

## 在Unity中添加的步骤：

### 方法1：复制现有的设置（最快）

1. 打开 **Title** 场景
2. 选择 **StartButton**
3. 在Inspector中找到 **Event Trigger** 组件
4. 右键点击组件标题 → **Copy Component**
5. 选择需要添加hover效果的按钮
6. 右键点击Inspector空白处 → **Paste Component As New**

### 方法2：手动添加

对于每个需要添加hover的按钮：

1. 选择按钮GameObject
2. 点击 **Add Component**
3. 搜索并添加 **Event Trigger**
4. 点击 **Add New Event Type**，选择 **Pointer Enter**
5. 点击 **+** 添加回调
6. 将场景中的 **CursorManager** (或UICursorHover组件所在的对象) 拖到Object字段
7. 在下拉菜单选择 **UICursorHover → OnMouseEnter**
8. 再次点击 **Add New Event Type**，选择 **Pointer Exit**
9. 点击 **+** 添加回调
10. 将同一个对象拖到Object字段
11. 在下拉菜单选择 **UICursorHover → OnMouseExit**

## 确认UICursorHover组件设置：

每个场景应该有一个GameObject (通常叫 CursorManager) 包含：
- **UICursorHover** 脚本
- **Cursor Texture** 字段设置为 **CursorCover** 图片

## 测试：

运行游戏，鼠标移到按钮上时应该：
- 光标变成 CursorCover 样式
- 鼠标移开后变回 CursorOriginalLook 样式
