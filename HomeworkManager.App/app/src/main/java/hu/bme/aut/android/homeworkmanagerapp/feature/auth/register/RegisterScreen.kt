package hu.bme.aut.android.homeworkmanagerapp.feature.auth.register

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Key
import androidx.compose.material.icons.filled.Person
import androidx.compose.material3.Button
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Scaffold
import androidx.compose.material3.SnackbarHost
import androidx.compose.material3.SnackbarHostState
import androidx.compose.material3.Text
import androidx.compose.material3.TextFieldDefaults
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.remember
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import androidx.hilt.navigation.compose.hiltViewModel
import hu.bme.aut.android.homeworkmanagerapp.R
import hu.bme.aut.android.homeworkmanagerapp.ui.common.BottomTextButton
import hu.bme.aut.android.homeworkmanagerapp.ui.common.NormalTextField
import hu.bme.aut.android.homeworkmanagerapp.ui.common.PasswordTextField
import kotlinx.coroutines.launch

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun RegisterScreen(
    modifier: Modifier = Modifier,
    viewModel: RegisterViewModel = hiltViewModel(),
    onRegister: () -> Unit,
    onLoginClick: () -> Unit,
) {
    val scope = rememberCoroutineScope()
    val snackBarHostState = remember {
        SnackbarHostState()
    }

    val registerUiState by viewModel.registerUiState.collectAsState()

    Scaffold(
        topBar = {},
        bottomBar = {},
        snackbarHost = {
            SnackbarHost(hostState = snackBarHostState)
        },
        containerColor = MaterialTheme.colorScheme.background
    ) { padding ->
        Box(
            modifier = modifier
                .fillMaxSize()
                .padding(padding)
                .background(MaterialTheme.colorScheme.background),
            contentAlignment = Alignment.Center,
        ) {
            Column(horizontalAlignment = Alignment.CenterHorizontally) {
                NormalTextField(
                    value = registerUiState.firstName,
                    label = stringResource(id = R.string.text_field_label_first_name),
                    onValueChange = { newValue -> viewModel.updateFirstName(newValue) },
                    isError = registerUiState.isFirstNameError,
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
                    value = registerUiState.lastName,
                    label = stringResource(id = R.string.text_field_label_last_name),
                    onValueChange = { newValue -> viewModel.updateLastName(newValue) },
                    isError = registerUiState.isLastNameError,
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
                    value = registerUiState.email,
                    label = stringResource(id = R.string.text_field_label_email),
                    onValueChange = { newValue -> viewModel.updateEmail(newValue) },
                    isError = registerUiState.isEmailError,
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
                    value = registerUiState.username,
                    label = stringResource(id = R.string.text_field_label_username),
                    onValueChange = { newValue -> viewModel.updateUsername(newValue) },
                    isError = registerUiState.isUsernameError,
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
                    value = registerUiState.password,
                    label = stringResource(id = R.string.text_field_label_password),
                    onValueChange = { newValue -> viewModel.updatePassword(newValue) },
                    isError = registerUiState.isPasswordError,
                    leadingIcon = {
                        Icon(
                            imageVector = Icons.Default.Key,
                            contentDescription = null,
                        )
                    },
                    isVisible = registerUiState.isPasswordVisible,
                    onVisibilityChanged = { viewModel.changePasswordVisibility() },
                    onDone = { },
                )
                Spacer(modifier = Modifier.height(10.dp))
                PasswordTextField(
                    value = registerUiState.confirmPassword,
                    label = stringResource(id = R.string.text_field_label_confirm_password),
                    onValueChange = { newValue -> viewModel.updateConfirmPassword(newValue) },
                    isError = registerUiState.isConfirmPasswordError,
                    leadingIcon = {
                        Icon(
                            imageVector = Icons.Default.Key,
                            contentDescription = null,
                        )
                    },
                    isVisible = registerUiState.isConfirmPasswordVisible,
                    onVisibilityChanged = { viewModel.changeConfirmPasswordVisibility() },
                    onDone = { },
                )
                Button(
                    onClick = {
                        viewModel.register(
                            onRegister = onRegister,
                            onError = {
                                scope.launch {
                                    snackBarHostState.showSnackbar("Register failed")
                                }
                            }
                        )
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
}
