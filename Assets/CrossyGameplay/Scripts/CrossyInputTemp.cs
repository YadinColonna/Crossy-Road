using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossyInputTemp : MonoBehaviour
{
    public CrossyCharacter _character;

    private void Update()
    {
        //Detect arrows
        //izquierda
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _character.Move(Vector2.left);
        }

        //derecha
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _character.Move(new Vector2(1f,0f));
        }

        //arriba
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _character.Move(Vector2.up);
        }

        //abajo
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _character.Move(Vector2.down);
        }
    }
}
