package hu.bme.aut.android.homeworkmanagerapp.feature.auth.register

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.width
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Key
import androidx.compose.material.icons.filled.Person
import androidx.compose.material3.Button
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.ui.common.BottomTextButton
import hu.bme.aut.android.homeworkmanagerapp.ui.common.NormalTextField
import hu.bme.aut.android.homeworkmanagerapp.ui.common.PasswordTextField

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun RegisterScreen(
    modifier: Modifier = Modifier,
    viewModel: RegisterViewModel = hiltViewModel(),
    onRegister: (String) -> Unit,
    onLoginClick: () -> Unit,
) {
    var emailValue by remember { mutableStateOf("") }
    var isEmailError by remember { mutableStateOf(false) }

    var usernameValue by remember { mutableStateOf("") }
    var isUsernameError by remember { mutableStateOf(false) }

    var passwordValue by remember { mutableStateOf("") }
    var isPasswordVisible by remember { mutableStateOf(false) }
    var isPasswordError by remember { mutableStateOf(false) }

    var confirmPasswordValue by remember { mutableStateOf("") }
    var isConfirmPasswordVisible by remember { mutableStateOf(false) }
    var isConfirmPasswordError by remember { mutableStateOf(false) }

    Box(
        modifier = modifier
            .fillMaxSize()
            .background(MaterialTheme.colorScheme.background),
        contentAlignment = Alignment.Center,
    ) {
        Column(horizontalAlignment = Alignment.CenterHorizontally) {
            NormalTextField(
                value = emailValue,
                label = stringResource(id = R.string.text_field_label_email),
                onValueChange = { newValue ->
                    emailValue = newValue
                    isEmailError = false
                },
                isError = isEmailError,
                leadingIcon = {
                    Icon(
                        imageVector = Icons.Default.Person,
                        contentDescription = null,
                    )
                },
                trailingIcon = { },
                onDone = { },
            )
            Spacer(modifier = Modifier.height(10.dp))
            NormalTextField(
                value = usernameValue,
                label = stringResource(id = R.string.text_field_label_username),
                onValueChange = { newValue ->
                    usernameValue = newValue
                    isUsernameError = false
                },
                isError = isUsernameError,
                leadingIcon = {
                    Icon(
                        imageVector = Icons.Default.Person,
                        contentDescription = null,
                    )
                },
                trailingIcon = { },
                onDone = { },
            )
            Spacer(modifier = Modifier.height(10.dp))
            PasswordTextField(
                value = passwordValue,
                label = stringResource(id = R.string.text_field_label_password),
                onValueChange = { newValue ->
                    passwordValue = newValue
                    isPasswordError = false
                },
                isError = isPasswordError,
                leadingIcon = {
                    Icon(
                        imageVector = Icons.Default.Key,
                        contentDescription = null,
                    )
                },
                isVisible = isPasswordVisible,
                onVisibilityChanged = { isPasswordVisible = !isPasswordVisible },
                onDone = { },
            )
            Spacer(modifier = Modifier.height(10.dp))
            PasswordTextField(
                value = confirmPasswordValue,
                label = stringResource(id = R.string.text_field_label_confirm_password),
                onValueChange = { newValue ->
                    confirmPasswordValue = newValue
                    isConfirmPasswordError = false
                },
                isError = isConfirmPasswordError,
                leadingIcon = {
                    Icon(
                        imageVector = Icons.Default.Key,
                        contentDescription = null,
                    )
                },
                isVisible = isConfirmPasswordVisible,
                onVisibilityChanged = { isConfirmPasswordVisible = !isConfirmPasswordVisible },
                onDone = { },
            )
            Button(
                onClick = {
                    if (emailValue.isEmpty() || !emailValue.contains(Regex("^[\\w-.]+@([\\w-]+\\.)+[\\w-]{2,4}\$"))) {
                        isEmailError = true
                    } else if (usernameValue.isEmpty()) {
                        isUsernameError = true
                    } else if (passwordValue.isEmpty()) {
                        isPasswordError = true
                    } else if (confirmPasswordValue.isEmpty() || passwordValue != confirmPasswordValue) {
                        isConfirmPasswordError = true
                    } else {
                        viewModel.register(
                            firstName = "",
                            lastName = "",
                            username = usernameValue,
                            password = passwordValue,
                            email = emailValue,
                            onSuccess = { onRegister(usernameValue) },
                            onError = {},
                        )
                    }
                },
                modifier = Modifier.width(TextFieldDefaults.MinWidth),
            ) {
                Text(text = stringResource(id = R.string.button_label_register))
            }
        }
        BottomTextButton(
            onClick = onLoginClick,
            label = stringResource(id = R.string.button_label_already_have_account),
            modifier = Modifier.align(Alignment.BottomCenter),
        )
    }
}
