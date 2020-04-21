using UnityEngine;
using UnityEngine.Audio;

public class MenuControl : MonoBehaviour
{
   public AudioMixer master;
   
   public void SetMusic(bool mute)
   {
      master.SetFloat("MusicVol", mute ? -3 : -80);
   }
   public void SetSfx(bool mute)
   {
      master.SetFloat("SFXVol", mute ? -0 : -80);
   }
}
