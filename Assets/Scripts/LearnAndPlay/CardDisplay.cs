
using UnityEngine;
using UnityEngine.UI;

public class CardDisplay : MonoBehaviour
{
    public ParticleSystem ps;
    public Card card;
    public Button cardButton;
    public Text animalType;
    public Text legs;
    public bool isFlying;
    public Text animalColor;
    public Image animalSprite;

    

    // Start is called before the first frame update
    void Start()
    {
        
        

        animalSprite.GetComponent<Image>().sprite = card.animalSprite;



    }
    private void OnValidate()
    {
        Refresh();
    }
    public void Refresh()
    {
        animalSprite.GetComponent<Image>().sprite = card.animalSprite;
 
    }


}
