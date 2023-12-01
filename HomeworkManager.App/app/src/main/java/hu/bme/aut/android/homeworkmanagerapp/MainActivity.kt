package hu.bme.aut.android.homeworkmanagerapp

import android.os.Build
import android.os.Bundle
import androidx.activity.compose.setContent
import androidx.annotation.RequiresApi
import androidx.appcompat.app.AppCompatActivity
import dagger.hilt.android.AndroidEntryPoint
import hu.bme.aut.android.homeworkmanagerapp.feature.HomeworkManagerApp
import hu.bme.aut.android.homeworkmanagerapp.ui.theme.HomeworkManagerAppTheme

@AndroidEntryPoint
class MainActivity : AppCompatActivity() {
    @RequiresApi(Build.VERSION_CODES.Q)
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContent {
            HomeworkManagerAppTheme {
                HomeworkManagerApp()
            }
        }
    }
}