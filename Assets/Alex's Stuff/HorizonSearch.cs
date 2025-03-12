using Palmmedia.ReportGenerator.Core;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class HorizonSearch : MonoBehaviour
{
    Función HorizonSearch(inicio, objetivo, horizonte_maximo):
    Inicializar frontera como una cola vacía
    Inicializar explorados como un conjunto vacío
    Agregar el estado inicial a la frontera

    Mientras la frontera no esté vacía:
        estado_actual = extraer de la frontera el primer elemento
        Si estado_actual es igual a objetivo:
            devolver "Objetivo encontrado"
        
        Si la profundidad de estado_actual es mayor que horizonte_maximo:
            continuar con el siguiente ciclo(ignorar este estado)


        Si estado_actual no está en explorados:
            Agregar estado_actual a explorados
            Generar los sucesores de estado_actual

            Para cada sucesor:
                Si no está en explorados:
                    Agregar sucesor a la frontera
    Fin Mientras

    devolver "Objetivo no encontrado dentro del horizonte"
Fin Función

}
