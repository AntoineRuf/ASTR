using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.SceneManagement;
using Assets.TBS_Framework.Scripts.ASTR.RogueSkills;

public class SquadBuilderController : MonoBehaviour {

    public Transform SquadPanel;
    public Transform SquadName;
    public Transform HelpPanel;
    public Sprite RogueImage;
    public Sprite MageImage;
    public Sprite WarriorImage;
    public Transform Tooltip;

    private Rogue _rogue;
    private Mage _mage;
    private Warrior _warrior;
    private TwinDaggers _twinDaggers;
    private ManaStrike _manaStrike;
    private AxeSlash _axeSlash;

    private Unit unit1Class;
    private Unit unit2Class;
    private Unit unit3Class;
    public List<Skill> AllSkills;



	// Use this for initialization
	void Start () {
        AllSkills = new List<Skill>();
        AllSkills = LoadMageSkills(AllSkills);
        AllSkills = LoadRogueSkills(AllSkills);
        AllSkills = LoadWarriorSkils(AllSkills);

        _mage = new Mage();
        _mage.CustomInitialize();
        _rogue = new Rogue();
        _rogue.CustomInitialize();
        _warrior = new Warrior();
        _warrior.CustomInitialize();

        _twinDaggers = new TwinDaggers();
        _manaStrike = new ManaStrike();
        _axeSlash = new AxeSlash();

        OnRogueButton1Clicked(1);
        OnMageButton1Clicked(2);
        OnWarriorButton1Clicked(3);
    }

    public void OnRogueButton1Clicked(int unitNumber)
    {
        string UnitPanelName = string.Format("Unit{0}", unitNumber);
        Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
        UnitPanel.FindChild("ClassName").GetComponent<Text>().text = "Rogue";
        UnitPanel.FindChild("ClassSprite").GetComponent<Image>().sprite = RogueImage;
        UnitPanel.FindChild("Description").GetComponent<Text>().text= "A swift fighter capable of getting behind enemies and deal massive damage to them.";
        UnitPanel.FindChild("ClassIcon").GetComponent<Image>().sprite = UnitPanel.FindChild("FirstClass").FindChild("Rogue").GetComponent<Image>().sprite;
        Transform UnitStatistics = UnitPanel.FindChild("Statistics");
        UnitStatistics.FindChild("HitPoints").GetComponent<Text>().text = string.Format("{0}", _rogue.TotalHitPoints);
        UnitStatistics.FindChild("MovementPoints").GetComponent<Text>().text = string.Format("{0}", _rogue.TotalMovementPoints);
        UnitStatistics.FindChild("Attack").GetComponent<Text>().text = string.Format("{0}-{1}", _twinDaggers.MinDamage, _twinDaggers.MaxDamage);
        UnitStatistics.FindChild("Initiative").GetComponent<Text>().text = string.Format("{0}", _rogue.Initiative);

        changeSkillPool(unitNumber, "Rogue");
        InitializeRogueSkills(unitNumber);
        affectClassToUnit(unitNumber, _rogue);
    }

    public void OnMageButton1Clicked(int unitNumber)
    {
        string UnitPanelName = string.Format("Unit{0}", unitNumber);
        Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
        UnitPanel.FindChild("ClassName").GetComponent<Text>().text = "Mage";
        UnitPanel.FindChild("ClassSprite").GetComponent<Image>().sprite = MageImage;
        UnitPanel.FindChild("Description").GetComponent<Text>().text = "Staying away from his enemies and dealing damage from afar is the main goal of the mage";
        UnitPanel.FindChild("ClassIcon").GetComponent<Image>().sprite = UnitPanel.FindChild("FirstClass").FindChild("Mage").GetComponent<Image>().sprite;
        Transform UnitStatistics = UnitPanel.FindChild("Statistics");
        UnitStatistics.FindChild("HitPoints").GetComponent<Text>().text = string.Format("{0}", _mage.TotalHitPoints);
        UnitStatistics.FindChild("MovementPoints").GetComponent<Text>().text = string.Format("{0}", _mage.TotalMovementPoints);
        UnitStatistics.FindChild("Attack").GetComponent<Text>().text = string.Format("{0}-{1}", _manaStrike.MinDamage, _twinDaggers.MaxDamage);
        UnitStatistics.FindChild("Initiative").GetComponent<Text>().text = string.Format("{0}", _mage.Initiative);

        changeSkillPool(unitNumber, "Mage");
        InitializeMageSkills(unitNumber);
        affectClassToUnit(unitNumber, _mage);
    }

    public void OnWarriorButton1Clicked(int unitNumber)
    {
        string UnitPanelName = string.Format("Unit{0}", unitNumber);
        Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
        UnitPanel.FindChild("ClassName").GetComponent<Text>().text = "Warrior";
        UnitPanel.FindChild("ClassSprite").GetComponent<Image>().sprite = WarriorImage;
        UnitPanel.FindChild("Description").GetComponent<Text>().text = "The warrior is pretty useless if staying away of the fight, but deals trendemous damage up close.";
        UnitPanel.FindChild("ClassIcon").GetComponent<Image>().sprite = UnitPanel.FindChild("FirstClass").FindChild("Warrior").GetComponent<Image>().sprite;
        Transform UnitStatistics = UnitPanel.FindChild("Statistics");
        UnitStatistics.FindChild("HitPoints").GetComponent<Text>().text = string.Format("{0}", _warrior.TotalHitPoints);
        UnitStatistics.FindChild("MovementPoints").GetComponent<Text>().text = string.Format("{0}", _warrior.TotalMovementPoints);
        UnitStatistics.FindChild("Attack").GetComponent<Text>().text = string.Format("{0}-{1}", _axeSlash.MinDamage, _twinDaggers.MaxDamage);
        UnitStatistics.FindChild("Initiative").GetComponent<Text>().text = string.Format("{0}", _warrior.Initiative);

        changeSkillPool(unitNumber, "Warrior");
        InitializeWarriorSkills(unitNumber);
        affectClassToUnit(unitNumber, _warrior);
    }

    public void retrieveSkills(int unitNumber)
    {
        string UnitPanelName = string.Format("Unit{0}", unitNumber);
        Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
        List<Skill> unitSkills = new List<Skill>();

        for (int i = 0; i < 4; ++i)
        {
            Transform item = UnitPanel.FindChild("Actifs Slots").FindChild("SkillSheet").GetChild(i).GetChild(0);
            unitSkills.Add(AllSkills.Find(s => (s.Name == item.name)));
            Debug.Log(item.name);
        }


    }

    public void InitializeMageSkills(int unitNumber)
    {
        string UnitPanelName = string.Format("Unit{0}", unitNumber);
        Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
        List<string> SkillNames = new List<string>();
        SkillNames.Add("Incinerate");
        SkillNames.Add("Fire Rain");
        SkillNames.Add("Ice Lance");
        SkillNames.Add("Headvice");
        for (int i = 0; i < 4; ++i)
        {
            Transform item = UnitPanel.FindChild("Actifs Slots").FindChild("SkillSheet").GetChild(i).GetChild(0);
            item.name = SkillNames[i];
            Sprite SkillSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TBS Framework/SkillsImages/" + SkillNames[i] + ".png");
            item.GetComponent<Image>().sprite = SkillSprite;
        }

    }

    public void InitializeRogueSkills(int unitNumber)
    {
        string UnitPanelName = string.Format("Unit{0}", unitNumber);
        Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
        List<string> SkillNames = new List<string>();
        SkillNames.Add("Snake Venom");
        SkillNames.Add("Backstab");
        SkillNames.Add("Lethal Toxin");
        SkillNames.Add("Evasive Maneuver");
        for (int i = 0; i < 4; ++i)
        {
            Transform item = UnitPanel.FindChild("Actifs Slots").FindChild("SkillSheet").GetChild(i).GetChild(0);
            item.name = SkillNames[i];
            Sprite SkillSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TBS Framework/SkillsImages/" + SkillNames[i] + ".png");
            item.GetComponent<Image>().sprite = SkillSprite;
        }

    }

    public void InitializeWarriorSkills(int unitNumber)
    {
        string UnitPanelName = string.Format("Unit{0}", unitNumber);
        Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
        List<string> SkillNames = new List<string>();
        SkillNames.Add("Raging Bull");
        SkillNames.Add("Shield Bash");
        SkillNames.Add("Shattering Force");
        SkillNames.Add("Whirlwind");
        for (int i = 0; i < 4; ++i)
        {
            Transform item = UnitPanel.FindChild("Actifs Slots").FindChild("SkillSheet").GetChild(i).GetChild(0);
            item.name = SkillNames[i];
            Sprite SkillSprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/TBS Framework/SkillsImages/" + SkillNames[i] + ".png");
            item.GetComponent<Image>().sprite = SkillSprite;
        }

    }

    public void changeSkillPool(int unitNumber, string poolName)
    {
        string UnitPanelName = string.Format("Unit{0}", unitNumber);
        Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
        switch (poolName)
        {
            case "Mage":
                UnitPanel.FindChild("SkillPoolWarrior").gameObject.SetActive(false);
                UnitPanel.FindChild("SkillPoolRogue").gameObject.SetActive(false);
                UnitPanel.FindChild("SkillPoolMage").gameObject.SetActive(true);
                break;
            case "Rogue":
                UnitPanel.FindChild("SkillPoolWarrior").gameObject.SetActive(false);
                UnitPanel.FindChild("SkillPoolRogue").gameObject.SetActive(true);
                UnitPanel.FindChild("SkillPoolMage").gameObject.SetActive(false);
                break;
            case "Warrior":
                UnitPanel.FindChild("SkillPoolWarrior").gameObject.SetActive(true);
                UnitPanel.FindChild("SkillPoolMage").gameObject.SetActive(false);
                UnitPanel.FindChild("SkillPoolRogue").gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    public void affectClassToUnit(int unitNumber, Unit classUnit)
    {
        switch (unitNumber)
        {
            case 1:
                unit1Class = classUnit;
                break;
            case 2:
                unit2Class = classUnit;
                break;
            case 3:
                unit3Class = classUnit;
                break;
            default:
                break;
        }
    }

    public List<Skill> LoadMageSkills(List<Skill> list)
    {
        list.Add(new ManaStrike());
        list.Add(new IceLance());
        list.Add(new VoidArmor());
        list.Add(new FireRain());
        list.Add(new Incinerate());
        list.Add(new Purify());
        list.Add(new Headvice());
        list.Add(new ChangingWinds());
        return list;
    }

    public List<Skill> LoadRogueSkills(List<Skill> list)
    {
        list.Add(new TwinDaggers());
        list.Add(new QuickDash());
        list.Add(new FanOfKnives());
        list.Add(new LethalToxin());
        list.Add(new ShrapnelMine());
        list.Add(new SnakeVenom());
        list.Add(new CloakAndDagger());
        return list;
    }

    public List<Skill> LoadWarriorSkils(List<Skill> list)
    {
        list.Add(new AxeSlash());
        list.Add(new Galvanize());
        list.Add(new RagingBull());
        list.Add(new SecondWind());
        list.Add(new Whirlwind());
        list.Add(new ShatteringForce());
        list.Add(new ComeBackHere());
        list.Add(new ShieldBash());
        return list;
    }

    public void printSkillTooltip(string skillName)
    {
        Skill skill = AllSkills.Find(s => (s.Name == skillName));
        Debug.Log(AllSkills.Find(s => (s.Name == skillName)));
        string name = skill.Name;
        string text = skill.Tooltip;
        int CD = skill.Cooldown;
        int minDamage = skill.MinDamage;
        int maxDamage = skill.MaxDamage;
        int minRange = skill.MinRange;
        int maxRange = skill.MaxRange;
        Tooltip.gameObject.SetActive(true);
        Tooltip.FindChild("Name").GetComponent<Text>().text = name;
        Tooltip.FindChild("Description").GetComponent<Text>().text = text;
        Tooltip.FindChild("CD").GetComponent<Text>().text = string.Format("{0}", CD);
        Tooltip.FindChild("Range").GetComponent<Text>().text = string.Format("{0}-{1}", minRange, maxRange);
        Tooltip.FindChild("Damage").GetComponent<Text>().text = string.Format("{0}-{1}", minDamage, maxDamage);
    }

    public void deleteSkillTooltip()
    {
        Tooltip.gameObject.SetActive(false);
    }

    public void OnBackButtonClicked()
    {
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);
    }

    public void OnSaveButtonClicked()
    {
        // if squad name is empty, return;
        if (SquadName.GetComponent<InputField>().text == "")
            return;
        PlayerData squadData = new PlayerData();
        squadData.squadName = SquadName.GetComponent<InputField>().text;
        List<UnitData> unitsData = new List<UnitData>();
        for (int j = 1; j < 4; ++j)
        {
            UnitData currentUnitData = new UnitData();
            string UnitPanelName = string.Format("Unit{0}", j);
            Transform UnitPanel = SquadPanel.FindChild(UnitPanelName);
            List<Skill> unitSkills = new List<Skill>();
            for (int i = 0; i < 4; ++i)
            {
                Transform item = UnitPanel.FindChild("Actifs Slots").FindChild("SkillSheet").GetChild(i).GetChild(0);
                unitSkills.Add(AllSkills.Find(s => (s.Name == item.name)));
            }
            currentUnitData.Skills = unitSkills;
            Debug.Log(currentUnitData.Skills[0]);
            currentUnitData.Class = UnitPanel.FindChild("ClassName").GetComponent<Text>().text;
            currentUnitData.Name = UnitPanel.Find("UnitName").GetComponent<InputField>().text;
            unitsData.Add(currentUnitData);
        }
        squadData.playerData = unitsData;
        GameControl.Save(squadData);
        PlayerData squad1 = GameControl.playerData[0];
        GameObject go = new GameObject();
        DontDestroyOnLoad(go);
        go.AddComponent<SquadSelectionToFightScene>();
        go.GetComponent<SquadSelectionToFightScene>().squad1 = squad1;
        SceneManager.LoadScene("TitleScreen", LoadSceneMode.Single);

        

    }

    public void OnLoadButtonClicked()
    {
        GameControl.Load();
        List<PlayerData> text = GameControl.playerData;
    }

    public void OnHelpButtonClicked()
    {
        HelpPanel.gameObject.SetActive(!HelpPanel.gameObject.activeSelf);
    }
        
}
