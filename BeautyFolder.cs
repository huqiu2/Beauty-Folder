using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.ShellExtensions;
using System;
using System.Runtime.InteropServices;

[ComVisible(true)]
[ClassInterface(ClassInterfaceType.AutoDual)]
[Guid("a4d1d84d-bee3-4a77-b737-2c2d4c9af555")] // 替换为您的GUID
[ProgId("BeautyFolder.ShellExtension")]
[ComDefaultInterface(typeof(IContextMenu))]
public class CustomContextMenu : IContextMenu
{
    public CustomContextMenu()
    {
    }

    public void QueryContextMenu(HMENU hMenu, uint indexMenu, uint idCmdFirst, uint idCmdLast, uint uFlags)
    {
        // 添加菜单项
        ShellUtil.AppendMenu(hMenu, indexMenu, MF_STRING, idCmdFirst + 1, "美化文件夹图标");
    }

    public void InvokeCommand(CMINVOKECOMMANDINFOEX info)
    {
        if (info.lpVerb == "美化文件夹图标")
        {
            // 获取选中的文件
            var selectedItems = ShellFile.FromFilePaths(info.LPWSTR);
            foreach (var item in selectedItems)
            {
                // 这里可以添加代码来处理文件，例如创建desktop.ini
                CreateDesktopIni(item.Path);
            }
        }
    }

    public void GetCommandString(uint idcmd, uint uflags, uint reserved, StringBuilder commandstring, int cch)
    {
    }

    private void CreateDesktopIni(string filePath)
    {
        // 创建或修改desktop.ini文件
        string directoryPath = System.IO.Path.GetDirectoryName(filePath);
        string desktopIniPath = System.IO.Path.Combine(directoryPath, "desktop.ini");

        // 检查desktop.ini是否存在
        if (!System.IO.File.Exists(desktopIniPath))
        {
            System.IO.File.Create(desktopIniPath).Close();
        }

        // 设置desktop.ini的属性
        System.IO.File.SetAttributes(desktopIniPath, System.IO.FileAttributes.Hidden);

        // 写入desktop.ini内容
        string content = "[.ShellClassInfo]\r\nIconFile=" + filePath + "\r\nIconIndex=0";
        System.IO.File.WriteAllText(desktopIniPath, content, System.Text.Encoding.UTF8);
    }
}
