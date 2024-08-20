using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace NumMayor6011464
{
    public partial class MainPage : ContentPage
    {
        private readonly LocalDbService _dbService;
        private int _editResultadoId;

        public MainPage(LocalDbService dbService)
        {
            InitializeComponent();
            _dbService = dbService;
            Task.Run(async () => listview.ItemsSource = await _dbService.GetResultado());
        }

        private async void sumarBtn_Clicked(object sender, EventArgs e)
        {
            // Validar que los campos de entrada no estén vacíos
            if (string.IsNullOrWhiteSpace(EntryPrimerNumero.Text) || string.IsNullOrWhiteSpace(EntrySegundoNumero.Text))
            {
                await DisplayAlert("Error", "Por favor ingresa ambos números.", "OK");
                return;
            }

            // Convertir los valores de las entradas a enteros
            if (!int.TryParse(EntryPrimerNumero.Text, out int numero1) ||
                !int.TryParse(EntrySegundoNumero.Text, out int numero2))
            {
                await DisplayAlert("Error", "Por favor ingresa números válidos.", "OK");
                return;
            }

            Suma suma = new Suma
            {
                Numero1 = numero1,
                Numero2 = numero2
            };

            var SumaTotal = suma.RealizarSuma();

            // Comparar los dos números y determinar cuál es mayor
            string resultadoComparacion;
            if (suma.Numero1 > suma.Numero2)
            {
                resultadoComparacion = $"El primer número ({suma.Numero1}) es mayor que el segundo número ({suma.Numero2}).";
            }
            else if (suma.Numero1 < suma.Numero2)
            {
                resultadoComparacion = $"El segundo número ({suma.Numero2}) es mayor que el primer número ({suma.Numero1}).";
            }
            else
            {
                resultadoComparacion = "Ambos números son iguales.";
            }

            // Guardar en la base de datos
            if (_editResultadoId == 0)
            {
                await _dbService.Create(new Resul
                {
                    Numero1 = EntryPrimerNumero.Text,
                    Numero2 = EntrySegundoNumero.Text,
                    Suma = SumaTotal.ToString(),
                });

                // Mostrar el resultado de la comparación
                labelMayor.Text = resultadoComparacion;
                labelresultado.Text = $"Suma: {SumaTotal}";
            }
            else
            {
                await _dbService.Update(new Resul
                {
                    Id = _editResultadoId,
                    Numero1 = EntryPrimerNumero.Text,
                    Numero2 = EntrySegundoNumero.Text,
                    Suma = SumaTotal.ToString(),
                });

                // Limpiar campos y mostrar el resultado de la comparación
                EntryPrimerNumero.Text = string.Empty;
                EntrySegundoNumero.Text = string.Empty;
                labelresultado.Text = $"Suma: {SumaTotal}";
                labelMayor.Text = resultadoComparacion;
                _editResultadoId = 0;
            }

            listview.ItemsSource = await _dbService.GetResultado();
        }

        private async void listview_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var resultado = (Resul)e.Item;
            var action = await DisplayActionSheet("Action", "Cancel", null, "Edit", "Delete");

            switch (action)
            {
                case "Edit":
                    _editResultadoId = resultado.Id;
                    EntryPrimerNumero.Text = resultado.Numero1;
                    EntrySegundoNumero.Text = resultado.Numero2;
                    labelresultado.Text = resultado.Suma;
                    labelMayor.Text = string.Empty; // Limpiar el resultado de comparación

                    break;

                case "Delete":
                    await _dbService.Delete(resultado);
                    listview.ItemsSource = await _dbService.GetResultado();
                    break;
            }
        }
    }
}
