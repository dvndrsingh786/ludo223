using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class EditProfileController : MonoBehaviour
{

    public GameObject changeName;
    public GameObject gridView;
    public GameObject buttonPrefab;

    private string avatarIndex;

    public GameObject PlayerNameMain;
    public GameObject PlayerAvatarMain;

    public GameObject editProfleAvatar;

    private StaticGameVariablesController staticController;


    private List<GameObject> buttons = new List<GameObject>();
    // Use this for initialization
    void Start()
    {

        avatarIndex = "0";
        if (PlayerNameMain != null)
        {
            avatarIndex = GameManager.Instance.myPlayerData.GetAvatarIndex();
            editProfleAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;
            changeName.GetComponent<InputField>().text = GameManager.Instance.nameMy;
        }

        staticController = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>();


        if (GameManager.Instance.facebookAvatar != null)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.GetComponent<ProfilePictureController>().picture.GetComponent<Image>().sprite = GameManager.Instance.facebookAvatar;
            button.transform.SetParent(gridView.transform, false);

            GameObject border = button.GetComponent<ProfilePictureController>().frame;
            GameObject picture = button.GetComponent<ProfilePictureController>().picture;
            if (GameManager.Instance.myPlayerData.GetAvatarIndex().Equals("fb"))
            {
                border.GetComponent<Image>().color = Color.green;
            }

            string index = "fb";
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => ClickButton(index, border, picture));

            buttons.Add(border);
        }



        for (int i = 0; i < staticController.avatars.Length; i++)
        {
            GameObject button = Instantiate(buttonPrefab);
            button.GetComponent<ProfilePictureController>().picture.GetComponent<Image>().sprite = staticController.avatars[i];
            button.transform.SetParent(gridView.transform, false);

            GameObject border = button.GetComponent<ProfilePictureController>().frame;
            GameObject picture = button.GetComponent<ProfilePictureController>().picture;
            if (PlayerNameMain != null)
            {
                if (GameManager.Instance.myPlayerData.GetAvatarIndex().Equals(i + ""))
                {
                    border.GetComponent<Image>().color = Color.green;
                }
            }



            string index = "" + i;
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener(() => ClickButton(index, border, picture));

            buttons.Add(border);
        }
    }

    public void ClickButton(string avatarIndex, GameObject border, GameObject picture)
    {
        this.avatarIndex = avatarIndex;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<Image>().color = Color.white;

        }
        border.GetComponent<Image>().color = Color.green;
        editProfleAvatar.GetComponent<Image>().sprite = picture.GetComponent<Image>().sprite;
    }

    public void Save()
    {


        if (PlayerNameMain != null)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();
            data.Add(MyPlayerData.AvatarIndexKey, avatarIndex);
            data.Add(MyPlayerData.PlayerName, changeName.GetComponent<InputField>().text);
            GameManager.Instance.myPlayerData.UpdateUserData(data);
        }


        if (PlayerNameMain != null)
            PlayerNameMain.GetComponent<Text>().text = changeName.GetComponent<InputField>().text;
      //  GameManager.Instance.nameMy = changeName.GetComponent<InputField>().text;

        //GameManager.Instance.avatarIndex = this.avatarIndex;

        if (avatarIndex.Equals("fb"))
        {
            PlayerAvatarMain.GetComponent<Image>().sprite = GameManager.Instance.facebookAvatar;
            GameManager.Instance.avatarMy = GameManager.Instance.facebookAvatar;
        }
        else
        {
            if (PlayerAvatarMain != null)
                PlayerAvatarMain.GetComponent<Image>().sprite = staticController.avatars[int.Parse(avatarIndex)];
                GameManager.Instance.avatarMy = staticController.avatars[int.Parse(avatarIndex)];
              GameManager.Instance.avatarMy = staticController.avatars[int.Parse(avatarIndex)];
        }

        editProfleAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;
        if (PlayerNameMain == null)
            GameManager.Instance.facebookManager.GuestLogin();

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {

            transform.root.gameObject.SetActive(false);
        }
        else
            gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
