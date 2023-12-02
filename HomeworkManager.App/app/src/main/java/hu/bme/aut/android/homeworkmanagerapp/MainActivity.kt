package hu.bme.aut.android.homeworkmanagerapp

import android.os.Build
import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.activity.viewModels
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import dagger.hilt.android.AndroidEntryPoint
import hu.bme.aut.android.homeworkmanagerapp.feature.HomeworkManagerApp
import hu.bme.aut.android.homeworkmanagerapp.feature.appointment.grid.AppointmentGridViewModel
import hu.bme.aut.android.homeworkmanagerapp.ui.theme.HomeworkManagerAppTheme

@AndroidEntryPoint
class MainActivity : AppCompatActivity() {
    private val viewModel by viewModels<AppointmentGridViewModel>()

    @RequiresApi(Build.VERSION_CODES.Q)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            HomeworkManagerAppTheme {
                HomeworkManagerApp()
            }
        }
    }

    override fun onDestroy() {
        viewModel.stopConnection()
        super.onDestroy()
    }
}