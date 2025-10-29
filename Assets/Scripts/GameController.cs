using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections; 
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public TextMeshProUGUI targetNameText;
    public TextMeshProUGUI vidasText;
    public TextMeshProUGUI puntuacionText;
    public AudioManager audioManager;
    public TextMeshProUGUI descripcionText;
    public TextMeshProUGUI temporizador;

    public GameObject panelDerrota;
    public TextMeshProUGUI puntuacionFinalDerrota;
    public GameObject panelVictoria;
    public TextMeshProUGUI puntuacionFinalVictoria;


    private List<string> opcionesJuego = new List<string>() { "Empoleon", "talonflame", "tapu koko", "metagross", "tyranitar", "mimikyu" };
    private string targetABuscar;
    private int vidas = 4;
    private int puntuacion = 0;

    private float tiempoRestante;
    private bool temporizadorIniciado = false;
    [SerializeField] private float duracionTemporizador = 10f;
    [SerializeField] private float esperaRondas = 5f;
    private bool esperandoRonda = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generaSiguienteTarget();
        tiempoRestante = duracionTemporizador;
        temporizadorIniciado = true;
        ActualizaUI();
        panelDerrota.gameObject.SetActive(false);
        panelVictoria.gameObject.SetActive(false);
    }

    void Update(){
        if(temporizadorIniciado == true)
        {
            tiempoRestante -= Time.deltaTime;
            temporizador.text = "Tiempo restante: " + tiempoRestante.ToString("F1");

            if(tiempoRestante <= 0 && !esperandoRonda)
            {
                vidas--;
                audioManager.SonidoRespuesta(false);
                if (vidas == 0)
                {
                    GameOver();
                }
                else{
                    StartCoroutine(EsperarRondas());
                }
            }
        }
    }

    void ActualizaUI()
    {
        targetNameText.text = "Busca el Pokémon " + targetABuscar;
        vidasText.text = "Tienes " + vidas + " vidas";
        puntuacionText.text = "Puntuación: " + puntuacion;
    }

    public void OnTargetFound(string targetIdentificado)
    {
        

        if (targetIdentificado == targetABuscar)
        {
            //El jugador ha acertado. Suma puntos y genera nuevo elemento a buscar
            puntuacion++;
            Descripciones(targetIdentificado);
            audioManager.SonidoRespuesta(true);
            if(puntuacion >= 15)
            {
                Win();
            }
            if(!esperandoRonda)
            {
                StartCoroutine(EsperarRondas());
            }
        }
        else
        {
            // El jugador ha fallado
            vidas--;
            audioManager.SonidoRespuesta(false);
            if (vidas == 0 && !esperandoRonda)
            {
                // Game Over
                GameOver();
            }
            else{
                StartCoroutine(EsperarRondas());
            }
        }
        tiempoRestante = duracionTemporizador;
        ActualizaUI();
    }

    public void OnTargetLost()
    {
        descripcionText.gameObject.SetActive(false);
    }

    void generaSiguienteTarget()
    {
        int posAleatoria = Random.Range(0, opcionesJuego.Count);
        targetABuscar = opcionesJuego[posAleatoria];
        descripcionText.gameObject.SetActive(false);
    }
    
    void GameOver()
    {
        panelDerrota.SetActive(true);
        puntuacionFinalDerrota.text = "Puntuacion final: " + puntuacion;
    }

    void Win()
    {
        panelVictoria.SetActive(true);
        puntuacionFinalVictoria.text = "Puntuacion final: " + puntuacion;
    }

    public void Reiniciar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Descripciones(string Pokemon){
        descripcionText.gameObject.SetActive(true);

        switch(Pokemon)
        {
            case "Empoleon":
                descripcionText.text = "Empoleon — Tipo: Agua/Acero Su orgullo es tan fuerte como su pico de acero. Nada con elegancia y puede partir el hielo más duro con un solo golpe.";
                break;
            case "talonflame":
                descripcionText.text = "Talonflame — Tipo: Fuego/Volador Sus alas arden mientras surca el cielo. Ataca en picado a velocidades increíbles, incendiando el aire a su paso.";
                break;
            case "tapu koko":
                descripcionText.text = "Tapu Koko — Tipo: Eléctrico/Hada Guardián de Melemele. Se mueve con extrema rapidez y descarga rayos para poner a prueba la fuerza de los entrenadores.";
                break;
            case "metagross":
                descripcionText.text = "Metagross — Tipo: Acero/Psíquico Un superordenador viviente. Sus cuatro cerebros le permiten analizar estrategias con una inteligencia sobrehumana.";
                break;
            case "tyranitar":
                descripcionText.text = "Tyranitar — Tipo: Roca/Siniestro Su poder es tal que puede destruir montañas. Es territorial y no teme a ningún rival, ni siquiera a los legendarios.";
                break;
            case "mimikyu":
                descripcionText.text = "Mimikyu — Tipo: Fantasma/Hada Se oculta bajo un disfraz para parecerse a Pikachu. Nadie ha visto su verdadero cuerpo... y quienes lo hicieron, huyeron aterrados.";
                break;
            default:
                descripcionText.text = "";
                break;
        }
    }

    private IEnumerator EsperarRondas()
    {
        esperandoRonda = true;
        temporizadorIniciado = false;
        yield return new WaitForSeconds(esperaRondas);
        generaSiguienteTarget();
        tiempoRestante = duracionTemporizador;
        temporizadorIniciado = true;
        ActualizaUI();
        esperandoRonda = false;
    }

    
}
