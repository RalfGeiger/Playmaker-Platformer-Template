using UnityEngine;
using Text = UnityEngine.UI.Text;

namespace GGEZ
{

//---------------------------------------------------------------------------
// This script runs the demo of Perfect Pixel Sprite
//---------------------------------------------------------------------------
public class PpsDemo : MonoBehaviour
{

private enum DemoMode
{
    Character,
    Checkerboard,
}

public Camera MainCamera;
public Text TitleText;
public Text BodyText;
public Text SwitchModesButtonText;
private DemoMode mode = DemoMode.Character;
public Transform[] MovingObjects;

public GameObject[] CheckerboardObjects;
public GameObject[] CharacterObjects;

//---------------------------------------------------------------------------
// SwitchModes - Invoked by the button on top of the screen
//---------------------------------------------------------------------------
public void SwitchModes ()
    {
    this.statementIndex = 0;
    switch (this.mode)
        {
        case DemoMode.Character:
            {
            this.mode = DemoMode.Checkerboard;
            this.TitleText.text = "Checkerboard";
            this.SwitchModesButtonText.text = "View Character";
            }
            break;
        case DemoMode.Checkerboard:
            {
            this.mode = DemoMode.Character;
            this.TitleText.text = "Multi-Part Character";
            this.SwitchModesButtonText.text = "View Checkerboard";
            }
            break;
        }

    }

void Start ()
    {
    this.SwitchModes ();
    }

//---------------------------------------------------------------------------
// ShowNextStatement - Invoked by the 'more...' button
//---------------------------------------------------------------------------
private int statementIndex = 0;
public void ShowNextStatement ()
    {
    this.statementIndex++;
    }

private static void SetGroupActive (GameObject[] gameObjects, bool active)
    {
    foreach (var go in gameObjects)
        {
        go.SetActive (active);
        }
    }

void Update ()
    {

    // Set the camera's position and size to be pixel-aligned correctly and really zoomed-in
    this.MainCamera.orthographicSize = (this.MainCamera.pixelHeight * 0.5f / (3 * 16f));
    this.MainCamera.transform.position = Vector3.back * 10;

    // Make only the objects we want for this mode visible to the camera
    SetGroupActive (this.CheckerboardObjects, this.mode == DemoMode.Checkerboard);
    SetGroupActive (this.CharacterObjects, this.mode == DemoMode.Character);

    const float speed = 0.5f;

    Vector3 circlingPosition = new Vector3 (
            1.00f * (Mathf.Cos (Time.time * speed)) + this.MainCamera.transform.position.x - 0.5f,
            0.25f * (Mathf.Sin (Time.time * speed)) + this.MainCamera.transform.position.y - 0.5f,
            0f
            );
    foreach (Transform t in this.MovingObjects)
        {
        t.position = circlingPosition;
        }

    // Update the demo text
    switch (this.mode)
        {
        case DemoMode.Checkerboard:
            {
            switch (this.statementIndex % 8)
                {
                case 0: this.BodyText.text = "This demo reveals texturing issues with sprites that are off the screen's pixel grid. Perfect Pixel Sprite fixes these by aligning your sprite container to this grid. Press \"more\" to keep reading..."; break;
                case 1: this.BodyText.text = "The checkerboard sprite is surrounded by pink to make edge bleeding obvious. The 'bilinear' row uses bilinear texture filtering and the 'point' row uses point texture filtering..."; break;
                case 2: this.BodyText.text = "The 'sliding' column is how sprites look without any adjustments. The 'fixed' column shows sprites that are aligned by Perfect Pixel Sprite..."; break;
                case 3: this.BodyText.text = "Watch the edges of the sliding-bilinear checkerboard (top left) as it circles. Do you notice how the texture seems disconnected from its edges? Compare that to how solid the fixed-bilinear version in the top right appears..."; break;
                case 4: this.BodyText.text = "Normally, this effect is hard to notice. This demo is zoomed in 3x to reveal it easily. However, point filtering causes a glitch that can be very obvious even at normal scale..."; break;
                case 5: this.BodyText.text = "Every once in a while, a pink line might appear on the border of the sliding-point checkerboard (bottom left). Not all platforms do this, but many do. Enable the <b>Pause If Pink</b> component on the Main Camera to catch the frame when it happens. Disable and unpause to continue..."; break;
                case 6: this.BodyText.text = "You might be wondering, \"Doesn't Unity have a Pixel Snap feature?\" The short answer is that it doesn't help. You can give that a try by changing the material of the sliding column to Sprites-Default-PixelSnapOn."; break;
                case 7: this.BodyText.text = "(click View Character for the other part of this demo)"; break;
                }
            }
            break;

        case DemoMode.Character:
            {
            switch (this.statementIndex % 4)
                {
                case 0: this.BodyText.text = "The king is made of 3 layers. As he moves, the texture issues seen only at the edges of a single sprite occur inside of the character..."; break;
                case 1: this.BodyText.text = "To get pixel-perfect rendering also requires care in setting up your texture assets and game camera. Check out ggez.org for details!"; break;
                case 2: this.BodyText.text = "(click View Checkerboard for the other part of this demo)"; break;
                }
            }
            break;
        }

    }
}
}
