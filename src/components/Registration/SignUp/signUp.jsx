import React from 'react';
import { Formik, Form, ErrorMessage } from "formik";
import { Input, ModalContainer, ModalContent, Title, Button, LoginButton } from './signUp.styled';
import * as Yup from 'yup'; // Импортируем Yup для валидации

// Определяем схему валидации
const validationSchema = Yup.object({
    username: Yup.string()
        .required('Username is required'), // Username обязателен
    email: Yup.string()
        .email('Invalid email address') // Проверка на корректность email
        .required('Email is required'), // Email обязателен
    password: Yup.string()
        .min(6, 'Password must be at least 6 characters') // Минимальная длина пароля — 6 символов
        .required('Password is required'), // Пароль обязателен
});

const SignUp = () => {
    return (
        <ModalContainer>
            <ModalContent>
                <Title>Sign Up</Title>
                <Formik
                    validationSchema={validationSchema}
                    initialValues={{ username: '', email: '', password: '' }}
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
                                <Input type="text" name="email" placeholder="Email" />
                                <ErrorMessage name="email" component="div" style={{ color: 'red' }} />
                            </div>
                            <div>
                                <Input type="password" name="password" placeholder="Password" />
                                <ErrorMessage name="password" component="div" style={{ color: 'red' }} />
                            </div>
                            <Button type="submit" disabled={isSubmitting}>Sign up</Button>
                            <LoginButton type="button">Sign In</LoginButton>
                        </Form>
                    )}
                </Formik>
            </ModalContent>
        </ModalContainer>
    );
}

export default SignUp;
