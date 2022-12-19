using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class UIPathData
{
    public enum UIType
    {
        TYPE_ITEM,          // ֻ������Դ·��
        TYPE_BASE,          // ��פ������UI��Close������ һ��UI
        TYPE_POP,           // ����ʽUI�����⣬��ǰֻ����һ���������� �������� ��һ��֮�� ��ֹ�ƶ� �޷���������UI
        TYPE_STORY,         // ���½��棬���½�����֣�����UI��ʧ�����½���رգ���������ָ�
        TYPE_TIP,           // �������� �ڶ���֮�� ������ ��ֹ�ƶ� �޷���������UI
        TYPE_MENUPOP,       // TYPE_POP��һ����֧ �����˵�MenuBar�򿪵Ķ���UI ��Ҫ���ڶ�̬���������������� ������POPUI��ȫһ��
        TYPE_MESSAGE,       // ��Ϣ��ʾUI ������֮�� һ������߲㼶 ������ ����ֹ�ƶ� �ɲ�������UI
        TYPE_DEATH,         // ����UI Ŀǰֻ�и������ ������Ӹ����������
    };

    public static Dictionary<string, UIPathData> m_DicUIInfo = new Dictionary<string, UIPathData>();
    public static Dictionary<string, UIPathData> m_DicUIName = new Dictionary<string, UIPathData>();

    // group���������ƣ��Ὣͬһ���ܵ�UI�����һ��
    // isMainAsset
    // bDestroyOnUnload ֻ��PopUI������
    public UIPathData(string _path, UIType _uiType, string groupName = null, bool bMainAsset = false, bool bDestroyOnUnload = true)
    {
        path = _path;
        uiType = _uiType;
        int lastPathPos = _path.LastIndexOf('/');
        if (lastPathPos > 0)
        {
            name = path.Substring(lastPathPos + 1);
        }
        else
        {
            name = path;
        }

        uiGroupName = groupName;
        isMainAsset = bMainAsset;

        isDestroyOnUnload = bDestroyOnUnload;

#if UNITY_ANDROID

        // < 768M UI�����л���
        if (SystemInfo.systemMemorySize < 768)
        {
            isDestroyOnUnload = true;
        }

#endif

        m_DicUIInfo.Add(_path, this);
        m_DicUIName.Add(name, this);
    }

    public string path;
    public string name;
    public UIType uiType;
    public string uiGroupName;
    public bool isMainAsset;            // �Ƿ�������Դ���������ԴUI���ر�
    public bool isDestroyOnUnload;
}
public class UIInfo
{
    // item
    public static UIPathData SysShopPageItem = new UIPathData("Prefab/UI/SGSysShopPageItem", UIPathData.UIType.TYPE_ITEM, "SystemShop");

    //baseui
    public static UIPathData TargetFrameRoot = new UIPathData("Prefab/UI/SGTargetFrameRoot", UIPathData.UIType.TYPE_BASE, "MainBaseUI");

    // STORY
    public static UIPathData StoryDialogRoot = new UIPathData("Prefab/UI/SGStoryDialogRoot", UIPathData.UIType.TYPE_STORY);

    // TIPBOX
    public static UIPathData ItemTooltipsRoot = new UIPathData("Prefab/UI/SGItemTooltipsRoot", UIPathData.UIType.TYPE_TIP, "TooltipsGroup");

    //MenuPop
    public static UIPathData Belle = new UIPathData("Prefab/UI/BelleController", UIPathData.UIType.TYPE_MENUPOP);

    //MessageUI
    public static UIPathData CentreNotice = new UIPathData("Prefab/UI/SGCentreNotice", UIPathData.UIType.TYPE_MESSAGE);


    //DeathUI
    public static UIPathData Relive = new UIPathData("Prefab/UI/SGRelive", UIPathData.UIType.TYPE_DEATH);
}
