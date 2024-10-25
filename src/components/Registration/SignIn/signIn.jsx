import React from 'react';
import { Formik, Form, ErrorMessage } from "formik";
import {Input, ModalContainer, ModalContent, Title, Button, SignUpButton} from './signIn.styled';
import * as Yup from 'yup'; // Импортируем Yup для валидации

// Определяем схему валидации
const validationSchema = Yup.object({
    username: Yup.string()
        .required('Username is required'), // Username обязателен
    password: Yup.string()
        .min(6, 'Password must be at least 6 characters') // Минимальная длина пароля — 6 символов
        .required('Password is required'), // Пароль обязателен
});

const SignIn = ({switchToSignUp}) => {
    return (
        <ModalContainer>
            <ModalContent>
                <Title>Sign In</Title>
                <Formik
                    validationSchema={validationSchema}
                    initialValues={{ username: '', password: '' }}
                    onSubmit={(values, actions) => {
                        console.log(values);
                        actions.setSubmitting(false);
                    }}>
                    {({ isSubmitting }) => (
                        <Form>
                            <div>
                                <Input type="text" name="username" placeholder="Username" />
                                <ErrorMessage name="username" component="div" style={{ color: 'red' }} />
                            </div>
                            <div>
                                <Input type="password" name="password" placeholder="Password" />
                                <ErrorMessage name="password" component="div" style={{ color: 'red' }} />
                            </div>
                            <Button type="submit" disabled={isSubmitting}>Sign In</Button>
                            <SignUpButton type="button" onClick={switchToSignUp}>Sign Up</SignUpButton>
                        </Form>
                    )}
                </Formik>
            </ModalContent>
        </ModalContainer>
    );
}

export default SignIn;
